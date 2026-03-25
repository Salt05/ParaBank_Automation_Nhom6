using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using ParaBank_Automation.Pages;
using ParaBank_Automation.Utilities;
using System;

namespace ParaBank_Automation.Tests
{
    [TestFixture]
    public class SecurityTests : BaseTest
    {
        private RegisterPage registerPage;
        private ProfilePage profilePage;
        private LoginPage loginPage;
        private TransferPage transferPage;
        private OverviewPage overviewPage;

        // Đọc tài khoản john từ TestData
        private static readonly System.Collections.Generic.List<UserModel> _users =
            JsonHelper.ReadTestData<UserModel>("TestData/users.json");
        private static readonly SecurityData _securityData =
            JsonHelper.ReadTestDataObject<SecurityData>("TestData/security_data.json");
        private static readonly TransferData _transferData =
            JsonHelper.ReadTestDataObject<TransferData>("TestData/transfer_data.json");
        private static UserModel JohnAccount => _users.Find(u => u.username == "john")!;

        [SetUp]
        public void SecuritySetUp()
        {
            registerPage = new RegisterPage(driver);
            profilePage = new ProfilePage(driver);
            loginPage = new LoginPage(driver);
            transferPage = new TransferPage(driver);
            overviewPage = new OverviewPage(driver);
        }


        [Test]
        [Description("TC_F5.1_01: Xác minh lưu DB Profile (Persistence)")]
        public void TC_F5_1_01_ProfilePersistence()
        {
            // Arrange
            var user = JohnAccount;
            var data = _securityData.profileUpdate;
            loginPage.Login(user.username, user.password);
            profilePage.NavigateTo();
            
            // Act
            profilePage.UpdateCityAndPhone(data.city, data.phone);
            
            TestContext.WriteLine($"👉 Cập nhật: City={data.city}, Phone={data.phone}");

            // Logout and log back in
            driver.Navigate().GoToUrl("https://parabank.parasoft.com/parabank/logout.htm");
            loginPage.Login(user.username, user.password);
            profilePage.NavigateTo();

            // Assert
            Assert.That(profilePage.GetCityValue(), Is.EqualTo(data.city), "Dữ liệu City không được duy trì sau khi Logout!");
        }

        [Test]
        [Description("TC_F6.1_01: Spam Click (Race Condition)")]
        public void TC_F6_1_01_SpamClick()
        {
            // Arrange
            loginPage.Login(JohnAccount.username, JohnAccount.password); 

            transferPage.NavigateToTransferFunds();
            
            // CHỌN 2 TÀI KHOẢN KHÁC NHAU ĐỂ GIAO DỊCH
            string fromAccount = transferPage.GetFirstAvailableAccountId();
            string toAccount = transferPage.GetSecondAvailableAccountId();

            // Đo số dư TRƯỚC khi Spam (Của riêng tài khoản FromAccount)
            overviewPage.NavigateTo();
            decimal balanceBefore = overviewPage.GetBalanceForAccount(fromAccount);

            transferPage.NavigateToTransferFunds();
            transferPage.SelectFromAccount(fromAccount);
            transferPage.SelectToAccount(toAccount);
            transferPage.EnterAmount(_transferData.spamTransfer.amount);

            // Act: Spam Transfer N lần nhanh (Từ TestData)
            int spamCount = _transferData.spamTransfer.spamClickCount;
            TestContext.WriteLine($"👉 Spam Click x{spamCount} lần với số tiền: ${_transferData.spamTransfer.amount}");
            for (int i = 0; i < spamCount; i++)
            {
                try {
                	transferPage.ClickTransfer();
                } catch {
                    // Ignore elements detached due to navigates
                }
            }

            // Assert
            System.Threading.Thread.Sleep(2000); // Ép Driver đợi 2s để Backend ghi nhận hạch toán
            overviewPage.NavigateTo();
            decimal balanceAfter = overviewPage.GetBalanceForAccount(fromAccount);
            
            // Nếu hạch toán an toàn, số dư tài khoản nguồn chỉ được phép trừ ĐÚNG $1 
            decimal deductAmount = balanceBefore - balanceAfter;
            string logMessage = "BUG: Spam Click dẫn tới lặp hạch toán (Race Condition)! \n" +
                               $"👉 Số tiền tài khoản nguồn BAN ĐẦU: ${balanceBefore} \n" +
                               $"👉 Số tiền tài khoản nguồn SAU khi thực hiện: ${balanceAfter} \n" +
                               $"👉 Số tiền MUỐN chuyển: ${_transferData.spamTransfer.amount} \n" +
                               $"👉 Số tiền THỰC TẾ bị trừ: ${deductAmount}";
                               
            Assert.That(deductAmount, Is.EqualTo(decimal.Parse(_transferData.spamTransfer.amount)), logMessage);
        }

        private void openNewAccountPageCreateSecondAccount()
        {
            OpenNewAccountPage openPage = new OpenNewAccountPage(driver);
            openPage.NavigateTo();
            openPage.OpenNewAccount("SAVINGS");
        }

        [Test]
        [Description("TC_F6.2_01: Quản lý phiên (Broken Access Control)")]
        public void TC_F6_2_01_BrokenAccessControl()
        {
            // Arrange
            loginPage.Login(JohnAccount.username, JohnAccount.password);
            System.Threading.Thread.Sleep(2000); // Dừng lại 2s nhìn rõ Đăng nhập
            
            overviewPage.NavigateTo();
            System.Threading.Thread.Sleep(2000); // Dừng lại 2s nhìn bảng Overview
            string savedUrl = driver.Url;

            // Act
            driver.FindElement(By.LinkText("Log Out")).Click();
            System.Threading.Thread.Sleep(3000); // Dừng lại 3s sau khi Đăng Xuất (Log out)
            
            driver.Navigate().GoToUrl(savedUrl);
            System.Threading.Thread.Sleep(3000); // Dừng lại 3s để bạn nhìn xem có vào được cửa sau không

            // Assert
            // Kiểm tra xem có Form Customer Login (Bằng chứng hệ thống đã chặn chặn xem bảng tài khoản)
            bool isLoginSidebarDisplayed = driver.FindElements(By.Name("username")).Count > 0;
            
            Assert.That(isLoginSidebarDisplayed, Is.True, "BUG: Hệ thống đã cho phép đột nhập vào xem dữ liệu tài khoản sau khi Logout!");
        }

        [Test]
        [Description("TC_F6.3_01: Chèn mã độc (XSS Injection)")]
        public void TC_F6_3_01_XssInjection()
        {
            // Arrange
            var data = _securityData.xssPayload;
            loginPage.Login(JohnAccount.username, JohnAccount.password);
            profilePage.NavigateTo();

            TestContext.WriteLine($"👉 Chèn XSS: City='{data.city}', Phone='{data.phone}'");

            profilePage.UpdateCityAndPhone(data.city, data.phone);

            // Assert - check if standard navigation causes alert exception
            bool alertTriggered = false;
            try {
                driver.Navigate().Refresh();
                var alert = driver.SwitchTo().Alert();
                alertTriggered = true;
                alert.Dismiss();
            } catch (NoAlertPresentException) {
                alertTriggered = false;
            }

            Assert.That(alertTriggered, Is.False, "Mã độc XSS đã kích hoạt popup script alert!");
        }

        [Test]
        [Description("TC_F6.4_01: Tràn bộ nhớ (Integer Overflow)")]
        public void TC_F6_4_01_IntegerOverflow()
        {
            // Arrange
            loginPage.Login(JohnAccount.username, JohnAccount.password);
            transferPage.NavigateToTransferFunds();

            var overflowData = _transferData.overflowTransfer;
            TestContext.WriteLine($"👉 {overflowData.description}: Nhập số tiền = {overflowData.amount}");
            transferPage.EnterAmount(overflowData.amount);
            transferPage.ClickTransfer();

            // Assert – Kiểm tra xem hệ thống có bị Crash 500 (Internal Error) hay không
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            try
            {
                // Nếu hiện bảng kết quả thành công (Điều vô lý)
                var title = driver.FindElement(By.XPath("//div[@id='showResult']/h1")).Text;
                Assert.That(title, Is.Not.EqualTo("Transfer Complete!"), "BUG: Hệ thống đã phê duyệt một con số tràn Integer!");
            }
            catch (NoSuchElementException)
            {
                // Nếu không hiện bảng kết quả chuẩn, kiểm tra xem có bị Crash không
                string pageText = driver.FindElement(By.TagName("body")).Text;
                Assert.That(pageText, Does.Not.Contain("An internal error has occurred"), "BUG: Máy chủ bị sập (Crash 500) khi nhập số tràn bộ nhớ Integer Overflow!");
            }
        }
    }
}
