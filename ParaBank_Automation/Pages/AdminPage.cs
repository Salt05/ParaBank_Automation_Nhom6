using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace ParaBank_Automation.Pages
{
    public class AdminPage : BasePage
    {
        // ===== LOCATORS =====
        private readonly By cleanButton = By.XPath("//button[@value='CLEAN']");
        private readonly By initializeButton = By.XPath("//button[@value='INIT']");

        public AdminPage(IWebDriver driver) : base(driver) { }

        public void NavigateTo()
        {
            driver.Navigate().GoToUrl("https://parabank.parasoft.com/parabank/admin.htm");
        }

        public void ClickCleanDatabase()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementToBeClickable(cleanButton));
            ClickElement(cleanButton);
        }

        public void ClickInitializeDatabase()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementToBeClickable(initializeButton));
            ClickElement(initializeButton);
        }
    }
}
