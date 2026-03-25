using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace ParaBank_Automation.Pages
{
    /// <summary>
    /// Page Object cho trang Register, kế thừa BasePage.
    /// Chứa các Locator và hàm thao tác trên form đăng ký.
    /// </summary>
    public class RegisterPage : BasePage
    {
        // ===== LOCATORS =====
        private readonly By firstNameField = By.Id("customer.firstName");
        private readonly By lastNameField = By.Id("customer.lastName");
        private readonly By addressField = By.Id("customer.address.street");
        private readonly By cityField = By.Id("customer.address.city");
        private readonly By stateField = By.Id("customer.address.state");
        private readonly By zipCodeField = By.Id("customer.address.zipCode");
        private readonly By phoneField = By.Id("customer.phoneNumber");
        private readonly By ssnField = By.Id("customer.ssn");
        private readonly By usernameField = By.Id("customer.username");
        private readonly By passwordField = By.Id("customer.password");
        private readonly By confirmPasswordField = By.Id("repeatedPassword");
        private readonly By registerButton = By.XPath("//input[@value='Register']");

        // Locator cho link Register trên trang chủ
        private readonly By registerLink = By.LinkText("Register");

        // Locators cho các thông báo lỗi (error message)
        private readonly By firstNameError = By.Id("customer.firstName.errors");
        private readonly By ssnError = By.Id("customer.ssn.errors");
        private readonly By confirmPasswordError = By.Id("repeatedPassword.errors");
        private readonly By usernameError = By.Id("customer.username.errors");

        // Locator cho thông báo Welcome sau khi đăng ký thành công
        private readonly By welcomeHeader = By.XPath("//h1[contains(@class,'title')]");

        public RegisterPage(IWebDriver driver) : base(driver) { }

        /// <summary>
        /// Điều hướng đến trang Register từ trang chủ.
        /// </summary>
        public void NavigateToRegisterPage()
        {
            driver.Navigate().GoToUrl("https://parabank.parasoft.com/parabank/register.htm");
        }

        /// <summary>
        /// Điền toàn bộ thông tin form đăng ký từ UserModel rồi click Register.
        /// Username được random hóa để tránh trùng lặp.
        /// </summary>
        public void RegisterNewUser(Utilities.UserModel user)
        {
            NavigateToRegisterPage();
            SendKeys(firstNameField, user.firstName);
            SendKeys(lastNameField, user.lastName);
            SendKeys(addressField, user.address);
            SendKeys(cityField, user.city);
            SendKeys(stateField, user.state);
            SendKeys(zipCodeField, user.zipCode);
            SendKeys(phoneField, user.phone);
            SendKeys(ssnField, user.ssn);
            SendKeys(usernameField, user.username);
            SendKeys(passwordField, user.password);
            SendKeys(confirmPasswordField, user.confirmPassword);
            ClickElement(registerButton);
        }

        /// <summary>
        /// Điền form đăng ký nhưng bỏ trống trường First Name.
        /// </summary>
        public void RegisterWithoutFirstName(Utilities.UserModel user)
        {
            NavigateToRegisterPage();
            // Bỏ trống firstName
            SendKeys(lastNameField, user.lastName);
            SendKeys(addressField, user.address);
            SendKeys(cityField, user.city);
            SendKeys(stateField, user.state);
            SendKeys(zipCodeField, user.zipCode);
            SendKeys(phoneField, user.phone);
            SendKeys(ssnField, user.ssn);
            SendKeys(usernameField, user.username);
            SendKeys(passwordField, user.password);
            SendKeys(confirmPasswordField, user.confirmPassword);
            ClickElement(registerButton);
        }

        /// <summary>
        /// Điền form đăng ký nhưng bỏ trống trường SSN.
        /// </summary>
        public void RegisterWithoutSSN(Utilities.UserModel user)
        {
            NavigateToRegisterPage();
            SendKeys(firstNameField, user.firstName);
            SendKeys(lastNameField, user.lastName);
            SendKeys(addressField, user.address);
            SendKeys(cityField, user.city);
            SendKeys(stateField, user.state);
            SendKeys(zipCodeField, user.zipCode);
            SendKeys(phoneField, user.phone);
            // Bỏ trống SSN
            SendKeys(usernameField, user.username);
            SendKeys(passwordField, user.password);
            SendKeys(confirmPasswordField, user.confirmPassword);
            ClickElement(registerButton);
        }

        /// <summary>
        /// Điền form đăng ký với Zip Code chứa chữ cái.
        /// </summary>
        public void RegisterWithInvalidZipCode(Utilities.UserModel user, string invalidZip)
        {
            NavigateToRegisterPage();
            SendKeys(firstNameField, user.firstName);
            SendKeys(lastNameField, user.lastName);
            SendKeys(addressField, user.address);
            SendKeys(cityField, user.city);
            SendKeys(stateField, user.state);
            SendKeys(zipCodeField, invalidZip);
            SendKeys(phoneField, user.phone);
            SendKeys(ssnField, user.ssn);
            SendKeys(usernameField, user.username);
            SendKeys(passwordField, user.password);
            SendKeys(confirmPasswordField, user.confirmPassword);
            ClickElement(registerButton);
        }

        /// <summary>
        /// Điền form đăng ký với Password và Confirm Password không khớp.
        /// </summary>
        public void RegisterWithMismatchedPasswords(Utilities.UserModel user, string password, string confirmPassword)
        {
            NavigateToRegisterPage();
            SendKeys(firstNameField, user.firstName);
            SendKeys(lastNameField, user.lastName);
            SendKeys(addressField, user.address);
            SendKeys(cityField, user.city);
            SendKeys(stateField, user.state);
            SendKeys(zipCodeField, user.zipCode);
            SendKeys(phoneField, user.phone);
            SendKeys(ssnField, user.ssn);
            SendKeys(usernameField, user.username);
            SendKeys(passwordField, password);
            SendKeys(confirmPasswordField, confirmPassword);
            ClickElement(registerButton);
        }

        /// <summary>
        /// Điền form đăng ký với Username cố định (dùng cho test trùng lặp username).
        /// </summary>
        public void RegisterWithFixedUsername(Utilities.UserModel user, string fixedUsername)
        {
            NavigateToRegisterPage();
            SendKeys(firstNameField, user.firstName);
            SendKeys(lastNameField, user.lastName);
            SendKeys(addressField, user.address);
            SendKeys(cityField, user.city);
            SendKeys(stateField, user.state);
            SendKeys(zipCodeField, user.zipCode);
            SendKeys(phoneField, user.phone);
            SendKeys(ssnField, user.ssn);
            SendKeys(usernameField, fixedUsername);
            SendKeys(passwordField, user.password);
            SendKeys(confirmPasswordField, user.confirmPassword);
            ClickElement(registerButton);
        }

        // ===== CÁC HÀM LẤY THÔNG BÁO =====

        /// <summary>
        /// Lấy text của thông báo Welcome (h1) sau khi đăng ký thành công.
        /// </summary>
        public string GetWelcomeMessage()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(welcomeHeader));
            return GetText(welcomeHeader);
        }

        /// <summary>
        /// Lấy thông báo lỗi First Name.
        /// </summary>
        public string GetFirstNameError()
        {
            return GetText(firstNameError);
        }

        /// <summary>
        /// Lấy thông báo lỗi SSN.
        /// </summary>
        public string GetSsnError()
        {
            return GetText(ssnError);
        }

        /// <summary>
        /// Lấy thông báo lỗi Confirm Password không khớp.
        /// </summary>
        public string GetConfirmPasswordError()
        {
            return GetText(confirmPasswordError);
        }

        /// <summary>
        /// Lấy thông báo lỗi Username (trùng lặp).
        /// </summary>
        public string GetUsernameError()
        {
            return GetText(usernameError);
        }

        /// <summary>
        /// Lấy thông báo lỗi chung (nằm trong phần tử có id chứa "error").
        /// Dùng cho các trường hợp lỗi chung không thuộc field cụ thể.
        /// </summary>
        public string GetGeneralErrorMessage()
        {
            var errorLocator = By.XPath("//span[@class='error']");
            return GetText(errorLocator);
        }

        /// <summary>
        /// Kiểm tra xem trang hiện tại có phải trang kết quả đăng ký thành công hay không.
        /// </summary>
        public bool IsRegistrationSuccessful()
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                wait.Until(ExpectedConditions.ElementIsVisible(welcomeHeader));
                return GetText(welcomeHeader).Contains("Welcome");
            }
            catch
            {
                return false;
            }
        }
    }
}
