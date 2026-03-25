using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace ParaBank_Automation.Pages
{
    /// <summary>
    /// Page Object cho trang Login, kế thừa BasePage.
    /// Chứa các Locator và hàm thao tác trên form đăng nhập.
    /// </summary>
    public class LoginPage : BasePage
    {
        // ===== LOCATORS =====
        private readonly By usernameField = By.Name("username");
        private readonly By passwordField = By.Name("password");
        private readonly By loginButton = By.XPath("//input[@value='Log In']");

        // Locator cho thông báo lỗi đăng nhập
        private readonly By loginErrorMessage = By.XPath("//p[@class='error']");

        // Locator cho link "Log Out" (dùng để xác minh đăng nhập thành công)
        private readonly By logOutLink = By.LinkText("Log Out");

        // Locator cho tiêu đề trang sau khi đăng nhập
        private readonly By pageTitle = By.XPath("//h1[contains(@class,'title')]");

        public LoginPage(IWebDriver driver) : base(driver) { }

        /// <summary>
        /// Đăng nhập với username và password.
        /// </summary>
        public void Login(string user, string pass)
        {
            SendKeys(usernameField, user);
            SendKeys(passwordField, pass);
            ClickElement(loginButton);
        }

        /// <summary>
        /// Đăng nhập chỉ với Password, bỏ trống Username.
        /// </summary>
        public void LoginWithoutUsername(string pass)
        {
            SendKeys(passwordField, pass);
            ClickElement(loginButton);
        }

        /// <summary>
        /// Đăng nhập chỉ với Username, bỏ trống Password.
        /// </summary>
        public void LoginWithoutPassword(string user)
        {
            SendKeys(usernameField, user);
            ClickElement(loginButton);
        }

        // ===== CÁC HÀM LẤY THÔNG BÁO =====

        /// <summary>
        /// Lấy thông báo lỗi đăng nhập hiển thị trên trang.
        /// </summary>
        public string GetLoginErrorMessage()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(loginErrorMessage));
            return GetText(loginErrorMessage);
        }

        /// <summary>
        /// Lấy tiêu đề lỗi (heading) khi đăng nhập thất bại.
        /// </summary>
        public string GetErrorTitle()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(pageTitle));
            return GetText(pageTitle);
        }

        /// <summary>
        /// Kiểm tra xem link "Log Out" có hiển thị không (đăng nhập thành công).
        /// </summary>
        public bool IsLogOutLinkDisplayed()
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                wait.Until(ExpectedConditions.ElementIsVisible(logOutLink));
                return driver.FindElement(logOutLink).Displayed;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Lấy text của link Log Out (dùng cho assertion).
        /// </summary>
        public string GetLogOutLinkText()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(logOutLink));
            return GetText(logOutLink);
        }
    }
}
