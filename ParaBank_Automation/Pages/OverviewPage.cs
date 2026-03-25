using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ParaBank_Automation.Pages
{
    public class OverviewPage : BasePage
    {
        // ===== LOCATORS =====
        private readonly By overviewLink = By.LinkText("Accounts Overview");
        private readonly By accountRows = By.XPath("//table[@id='accountTable']/tbody/tr[td/a]");
        private readonly By balanceCells = By.XPath("//table[@id='accountTable']/tbody/tr[td/a]/td[2]");
        private readonly By totalBalance = By.XPath("//table[@id='accountTable']//tr[td/b='Total']/td[2]/b");
        private readonly By accountDetailHeading = By.XPath("//h1[@class='title']");

        public OverviewPage(IWebDriver driver) : base(driver) { }

        public void NavigateTo()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementToBeClickable(overviewLink));
            ClickElement(overviewLink);
        }

        public int GetAccountCount()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(accountRows));
            return driver.FindElements(accountRows).Count;
        }

        public decimal CalculateTotalBalance()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(balanceCells));
            var balances = driver.FindElements(balanceCells);
            decimal sum = 0;

            foreach (var cell in balances)
            {
                string text = cell.Text.Replace("$", "").Replace(",", "").Trim();
                if (decimal.TryParse(text, out decimal val))
                {
                    sum += val;
                }
            }
            return sum;
        }

        public decimal GetTotalDisplay()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(totalBalance));
            string text = GetText(totalBalance).Replace("$", "").Replace(",", "").Trim();
            return decimal.Parse(text);
        }

        public void ClickAccountLink(string accountId)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            By locator = By.LinkText(accountId);
            wait.Until(ExpectedConditions.ElementToBeClickable(locator));
            ClickElement(locator);
        }

        public string GetAccountDetailHeading()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(accountDetailHeading));
            return GetText(accountDetailHeading);
        }

        public decimal GetBalanceForAccount(string accountId)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            By locator = By.XPath($"//table[@id='accountTable']//tr[td/a[text()='{accountId}']]/td[2]");
            wait.Until(ExpectedConditions.ElementIsVisible(locator));
            string text = GetText(locator).Replace("$", "").Replace(",", "").Trim();
            return decimal.Parse(text);
        }

        public string GetFirstAccountId()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            // Sửa XPath: Tìm thẻ a trong td đầu tiên của những dòng TR có đi kèm link Account
            By locator = By.XPath("//table[@id='accountTable']/tbody/tr[td/a]/td[1]/a");
            wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(locator));
            
            var rows = driver.FindElements(locator);
            return rows.First().Text;
        }
    }
}
