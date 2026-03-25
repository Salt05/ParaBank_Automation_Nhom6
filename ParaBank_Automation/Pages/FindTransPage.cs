using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;

namespace ParaBank_Automation.Pages
{
    public class FindTransPage : BasePage
    {
        // ===== LOCATORS =====
        private readonly By findTransactionsLink = By.LinkText("Find Transactions");
        private readonly By transactionIdField = By.Id("transactionId");
        private readonly By dateField = By.Id("transactionDate");
        private readonly By fromDateField = By.Id("fromDate");
        private readonly By toDateField = By.Id("toDate");
        private readonly By amountField = By.Id("amount");

        private readonly By findByIdButton = By.Id("findById");
        private readonly By findByDateButton = By.Id("findByDate");
        private readonly By findByDateRangeButton = By.Id("findByDateRange");
        private readonly By findByAmountButton = By.Id("findByAmount");

        private readonly By transactionResults = By.XPath("//table[@id='transactionTable']/tbody/tr");
        private readonly By firstTransactionId = By.XPath("//table[@id='transactionTable']/tbody/tr[1]/td[2]/a");

        public FindTransPage(IWebDriver driver) : base(driver) { }

        public void NavigateTo()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementToBeClickable(findTransactionsLink));
            ClickElement(findTransactionsLink);
        }

        public void FindById(string id)
        {
            SendKeys(transactionIdField, id);
            ClickElement(findByIdButton);
        }

        public void FindByDate(string date)
        {
            SendKeys(dateField, date);
            ClickElement(findByDateButton);
        }

        public void FindByDateRange(string fromDate, string toDate)
        {
            SendKeys(fromDateField, fromDate);
            SendKeys(toDateField, toDate);
            ClickElement(findByDateRangeButton);
        }

        public void FindByAmount(string amount)
        {
            SendKeys(amountField, amount);
            ClickElement(findByAmountButton);
        }

        public int GetResultCount()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            try {
                wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(transactionResults));
                return driver.FindElements(transactionResults).Count;
            } catch {
                return 0; // No results found
            }
        }

        public string GetFirstTransactionIdResult()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(firstTransactionId));
            return GetText(firstTransactionId);
        }
    }
}
