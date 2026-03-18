using NUnit.Framework;
using OpenQA.Selenium;
using ParaBank_Automation.Pages;

namespace ParaBank_Automation.Tests
{
    // Test class kiểm thử Login, kế thừa BaseTest
    public class LoginTests : BaseTest
    {
        [Test]
        public void LoginSuccessTest()
        {
            var loginPage = new LoginPage(driver);
            loginPage.Login("john", "demo");
            // Kiểm tra đăng nhập thành công bằng cách kiểm tra sự xuất hiện của link 'Log Out'
            var logOutLink = loginPage.GetText(By.XPath("//a[text()='Log Out']"));
            Assert.That(logOutLink, Is.EqualTo("Log Out"), "Đăng nhập thất bại hoặc không tìm thấy link Log Out.");
        }
    }
}
