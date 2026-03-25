using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace ParaBank_Automation.Pages
{
    public class OpenNewAccountPage : BasePage
    {
        private readonly By openNewAccountLink = By.LinkText("Open New Account");
        private readonly By accountTypeDropdown = By.Id("type");
        private readonly By fromAccountIdDropdown = By.Id("fromAccountId");
        private readonly By openNewAccountButton = By.XPath("//input[@value='Open New Account']");
        private readonly By successMessage = By.XPath("//div[@id='openAccountResult']//h1");

        public OpenNewAccountPage(IWebDriver driver) : base(driver) { }

        public void NavigateTo()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementToBeClickable(openNewAccountLink));
            ClickElement(openNewAccountLink);
        }

        public void OpenNewAccount(string type)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(accountTypeDropdown));
            var select = new SelectElement(driver.FindElement(accountTypeDropdown));
            select.SelectByText(type);

            // Give it some time for AJAX to load accounts
            wait.Until(d => new SelectElement(d.FindElement(fromAccountIdDropdown)).Options.Count > 0);

            ClickElement(openNewAccountButton);
            wait.Until(ExpectedConditions.ElementIsVisible(successMessage));
        }
    }
}
