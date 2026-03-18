using OpenQA.Selenium;

namespace ParaBank_Automation.Pages
{
    // Page Object cho trang Login, kế thừa BasePage
    public class LoginPage : BasePage
    {
        // Locators
        private readonly By usernameField = By.Name("username");
        private readonly By passwordField = By.Name("password");
        private readonly By loginButton = By.XPath("//input[@value='Log In']");

        public LoginPage(IWebDriver driver) : base(driver) { }

        // Action: Đăng nhập với user/pass
        public void Login(string user, string pass)
        {
            SendKeys(usernameField, user); // Nhập username
            SendKeys(passwordField, pass); // Nhập password
            ClickElement(loginButton);     // Click nút Login
        }
    }
}
