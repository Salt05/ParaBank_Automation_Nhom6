using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace ParaBank_Automation.Pages
{
    public class OpenAccountPage : BasePage
    {
        // ===== LOCATORS =====
        private readonly By openNewAccountLink = By.LinkText("Open New Account");
        private readonly By accountTypeDropdown = By.Id("type");
        private readonly By fromAccountIdDropdown = By.Id("fromAccountId");
        private readonly By openAccountButton = By.CssSelector("input[value='Open New Account']");
        private readonly By successMessage = By.XPath("//div[@id='openAccountResult']/h1");
        private readonly By newAccountIdLink = By.Id("newAccountId");

        public OpenAccountPage(IWebDriver driver) : base(driver) { }

        public void NavigateTo()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementToBeClickable(openNewAccountLink));
            ClickElement(openNewAccountLink);
        }

        public void SelectAccountType(string type)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(accountTypeDropdown));
            var select = new SelectElement(driver.FindElement(accountTypeDropdown));
            select.SelectByText(type.ToUpper());
        }

        public void SelectFromAccount(string accountId)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            // Chờ cho dropdown có dữ liệu (AJAX)
            wait.Until(d => new SelectElement(d.FindElement(fromAccountIdDropdown)).Options.Count > 0);
            var select = new SelectElement(driver.FindElement(fromAccountIdDropdown));
            select.SelectByValue(accountId);
        }

        public void ClickOpenAccount()
        {
            // Đôi khi nút bị che bởi AJAX load, dùng Wait
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementToBeClickable(openAccountButton));
            ClickElement(openAccountButton);
        }

        public string GetSuccessMessage()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(successMessage));
            return GetText(successMessage);
        }

        public string GetNewAccountId()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(newAccountIdLink));
            return GetText(newAccountIdLink);
        }

        public void ClickNewAccountId()
        {
            ClickElement(newAccountIdLink);
        }

        public string GetFirstAvailableAccountId()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => new SelectElement(d.FindElement(fromAccountIdDropdown)).Options.Count > 0);
            var select = new SelectElement(driver.FindElement(fromAccountIdDropdown));
            return select.Options[0].GetAttribute("value");
        }
    }
}
