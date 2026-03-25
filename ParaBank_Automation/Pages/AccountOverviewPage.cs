using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ParaBank_Automation.Pages
{
    public class AccountOverviewPage : BasePage
    {
        private readonly By accountsOverviewLink = By.LinkText("Accounts Overview");
        private readonly By accountBalanceCell = By.XPath("//table[@id='accountTable']//tr[td[a[text()='{0}']]]/td[2]");

        public AccountOverviewPage(IWebDriver driver) : base(driver) { }

        public void NavigateToOverview()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementToBeClickable(accountsOverviewLink));
            ClickElement(accountsOverviewLink);
        }

        public decimal GetAccountBalance(string accountId)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            By locator = By.XPath($"//table[@id='accountTable']//tr[td/a[text()='{accountId}']]/td[2]");
            wait.Until(ExpectedConditions.ElementIsVisible(locator));
            string balanceStr = GetText(locator).Replace("$", "").Replace(",", "").Trim();
            return decimal.Parse(balanceStr);
        }
    }
}
