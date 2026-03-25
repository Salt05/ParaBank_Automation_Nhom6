using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace ParaBank_Automation.Pages
{
    public class TransferPage : BasePage
    {
        // ===== LOCATORS =====
        private readonly By transferFundsLink = By.LinkText("Transfer Funds");
        private readonly By amountField = By.Id("amount");
        private readonly By fromAccountIdDropdown = By.Id("fromAccountId");
        private readonly By toAccountIdDropdown = By.Id("toAccountId");
        private readonly By transferButton = By.XPath("//input[@value='Transfer']");
        private readonly By transferResultTitle = By.XPath("//div[@id='showResult']/h1");
        private readonly By transferResultMessage = By.XPath("//div[@id='showResult']/p");

        public TransferPage(IWebDriver driver) : base(driver) { }

        public void NavigateToTransferFunds()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementToBeClickable(transferFundsLink));
            ClickElement(transferFundsLink);
        }

        public void EnterAmount(string amount)
        {
            SendKeys(amountField, amount);
        }

        public void SelectFromAccount(string accountId)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            // Wait for dropdown to have options (Explicit Wait requirement)
            wait.Until(d => new SelectElement(d.FindElement(fromAccountIdDropdown)).Options.Count > 0);
            
            var select = new SelectElement(driver.FindElement(fromAccountIdDropdown));
            select.SelectByValue(accountId);
        }

        public void SelectToAccount(string accountId)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            // Wait for dropdown to have options (Explicit Wait requirement)
            wait.Until(d => new SelectElement(d.FindElement(toAccountIdDropdown)).Options.Count > 0);

            var select = new SelectElement(driver.FindElement(toAccountIdDropdown));
            select.SelectByValue(accountId);
        }

        public void ClickTransfer()
        {
            ClickElement(transferButton);
        }

        public string GetTransferResultTitle()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(transferResultTitle));
            return GetText(transferResultTitle);
        }

        public string GetTransferResultMessage()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(transferResultMessage));
            return GetText(transferResultMessage);
        }

        public string GetFirstAvailableAccountId()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => new SelectElement(d.FindElement(fromAccountIdDropdown)).Options.Count > 0);
            var select = new SelectElement(driver.FindElement(fromAccountIdDropdown));
            return select.Options[0].GetAttribute("value");
        }

        public string GetSecondAvailableAccountId()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => new SelectElement(d.FindElement(fromAccountIdDropdown)).Options.Count > 1);
            var select = new SelectElement(driver.FindElement(fromAccountIdDropdown));
            return select.Options[1].GetAttribute("value");
        }
    }
}
