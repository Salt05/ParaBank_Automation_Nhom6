using NUnit.Framework;
using ParaBank_Automation.Pages;
using ParaBank_Automation.Utilities;
using System;

namespace ParaBank_Automation.Tests
{
    [TestFixture]
    public class AuthTests : BaseTest
    {
        private LoginPage loginPage;
        private RegisterPage registerPage;

        // Đọc dữ liệu từ TestData
        private static readonly System.Collections.Generic.List<UserModel> _users =
            JsonHelper.ReadTestData<UserModel>("TestData/users.json");
        private static readonly SecurityData _securityData =
            JsonHelper.ReadTestDataObject<SecurityData>("TestData/security_data.json");

        private static UserModel JohnAccount => _users.Find(u => u.username == "john")!;

        [SetUp]
        public void AuthSetUp()
        {
            loginPage = new LoginPage(driver);
            registerPage = new RegisterPage(driver);
        }

        [Test]
        [Description("TC_F1.1_01: Đăng ký hợp lệ (Luồng chuẩn)")]
        public void TC_F1_1_01_RegisterSuccess()
        {
            // Arrange: Tạo user ngẫu nhiên (Luồng này luôn cần random để không bị trùng)
            var randomStr = Guid.NewGuid().ToString().Substring(0, 8);
            var user = new UserModel
            {
                firstName = "First" + randomStr,
                lastName = "Last" + randomStr,
                address = "123 Street",
                city = "City",
                state = "State",
                zipCode = "12345",
                phone = "1234567890",
                ssn = "123-456-" + randomStr.Substring(0, 4),
                username = "user_" + randomStr,
                password = "Password123",
                confirmPassword = "Password123"
            };

            TestContext.WriteLine("--------------------------------------------------");
            TestContext.WriteLine("👉 ĐĂNG KÝ TÀI KHOẢN MỚI:");
            TestContext.WriteLine($"• Username: {user.username}");
            TestContext.WriteLine($"• Họ tên: {user.firstName} {user.lastName}");
            TestContext.WriteLine("--------------------------------------------------");

            // Act
            registerPage.RegisterNewUser(user);

            // Assert
            Assert.That(registerPage.IsRegistrationSuccessful(), Is.True, "Đăng ký không thành công!");
            Assert.That(registerPage.GetWelcomeMessage(), Contains.Substring(user.username), "Thông báo Welcome không khớp!");
        }

        [Test]
        [Description("TC_F1.1_02: Đăng ký với Username trùng lặp")]
        public void TC_F1_1_02_RegisterFail_DuplicateUsername()
        {
            // Arrange: Đọc dữ liệu từ TestData (username=john là đã có sẵn trong DB seeded)
            var dup = _securityData.duplicateUser;
            var randomStr = Guid.NewGuid().ToString().Substring(0, 4);
            var user = new UserModel
            {
                firstName = dup.firstName,
                lastName = dup.lastName,
                address = dup.address,
                city = dup.city,
                state = dup.state,
                zipCode = dup.zipCode,
                phone = dup.phone,
                ssn = "123-456-" + randomStr,   // SSN random để tránh lỗi SSN trùng
                username = dup.username,          // Tên trùng với john
                password = dup.password,
                confirmPassword = dup.confirmPassword
            };

            TestContext.WriteLine("--------------------------------------------------");
            TestContext.WriteLine($"👉 {dup.description}");
            TestContext.WriteLine($"• Username cố tình trùng: {user.username}");
            TestContext.WriteLine("--------------------------------------------------");

            // Act
            registerPage.RegisterNewUser(user);

            // Assert
            var error = registerPage.GetUsernameError();
            Assert.That(error, Is.EqualTo("This username already exists."), "Thông báo lỗi không khớp!");
        }

        [Test]
        [Description("TC_F1.2_01: Đăng nhập hợp lệ (Luồng chuẩn)")]
        public void TC_F1_2_01_LoginSuccess()
        {
            // Đọc tài khoản từ TestData
            var user = JohnAccount;

            TestContext.WriteLine("--------------------------------------------------");
            TestContext.WriteLine($"👉 ĐĂNG NHẬP: Username={user.username}");
            TestContext.WriteLine("--------------------------------------------------");

            loginPage.Login(user.username, user.password);

            Assert.That(loginPage.IsLogOutLinkDisplayed(), Is.True, "Đăng nhập không thành công!");
        }

        [Test]
        [Description("TC_F1.2_02: Đăng nhập sai Password (Bắt Bug Server)")]
        public void TC_F1_2_02_LoginFail_InvalidPassword()
        {
            // Đọc dữ liệu từ TestData security_data.json
            var failData = _securityData.loginFail;

            TestContext.WriteLine("--------------------------------------------------");
            TestContext.WriteLine($"👉 {failData.description}");
            TestContext.WriteLine($"• Username: {failData.username} | Mật khẩu sai: {failData.wrongPassword}");
            TestContext.WriteLine("--------------------------------------------------");

            loginPage.Login(failData.username, failData.wrongPassword);

            var error = loginPage.GetLoginErrorMessage();
            Assert.That(error, Is.EqualTo("The username and password could not be verified."), "Thông báo lỗi nhập sai pass không đúng!");
        }
    }
}
