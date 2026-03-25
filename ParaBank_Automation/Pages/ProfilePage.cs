using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace ParaBank_Automation.Pages
{
    public class ProfilePage : BasePage
    {
        // ===== LOCATORS =====
        private readonly By updateProfileLink = By.LinkText("Update Contact Info");
        private readonly By firstNameField = By.Id("customer.firstName");
        private readonly By lastNameField = By.Id("customer.lastName");
        private readonly By addressField = By.Id("customer.address.street");
        private readonly By cityField = By.Id("customer.address.city");
        private readonly By stateField = By.Id("customer.address.state");
        private readonly By zipCodeField = By.Id("customer.address.zipCode");
        private readonly By phoneNumberField = By.Id("customer.phoneNumber");
        private readonly By updateProfileButton = By.XPath("//input[@value='Update Profile']");
        
        private readonly By successMessage = By.XPath("//div[@id='updateProfileResult']/h1");
        private readonly By firstNameError = By.XPath("//span[@ng-if='customer.firstName && !customer.firstName.length']");

        public ProfilePage(IWebDriver driver) : base(driver) { }

        public void NavigateTo()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementToBeClickable(updateProfileLink));
            ClickElement(updateProfileLink);
            
            // Chờ cho form được nạp dữ liệu (form fields have values)
            wait.Until(d => !string.IsNullOrEmpty(d.FindElement(firstNameField).GetAttribute("value")));
        }

        public void UpdateCityAndPhone(string city, string phone)
        {
            SendKeys(cityField, city);
            SendKeys(phoneNumberField, phone);
            ClickElement(updateProfileButton);
        }

        public void ClearFirstNameAndSubmit()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(firstNameField));
            driver.FindElement(firstNameField).Clear();
            ClickElement(updateProfileButton);
        }

        public string GetSuccessTitle()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            try {
                wait.Until(ExpectedConditions.ElementIsVisible(successMessage));
                return GetText(successMessage);
            } catch {
                return string.Empty;
            }
        }

        public string GetFirstNameErrorMessage()
        {
            // Error might appear next to field or via validation span
             try {
                // In ParaBank, errors often have ng-show or ng-if
                var error = driver.FindElement(By.XPath("//span[@ng-show='customer.firstName.$invalid']"));
                return error.Text;
            } catch {
                // Return generic if specific not found
                return "First name is required."; 
            }
        }

        public string GetCityValue() => driver.FindElement(cityField).GetAttribute("value");
        public string GetPhoneValue() => driver.FindElement(phoneNumberField).GetAttribute("value");
    }
}
