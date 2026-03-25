using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace ParaBank_Automation.Pages
{
    public class LoanPage : BasePage
    {
        // ===== LOCATORS =====
        private readonly By requestLoanLink = By.LinkText("Request Loan");
        private readonly By loanAmountField = By.Id("amount");
        private readonly By downPaymentField = By.Id("downPayment");
        private readonly By fromAccountIdDropdown = By.Id("fromAccountId");
        private readonly By applyNowButton = By.XPath("//input[@value='Apply Now']");
        
        private readonly By loanStatus = By.Id("loanStatus");
        private readonly By loanResultMessage = By.XPath("//div[@id='requestLoanResult']/p");
        private readonly By newAccountIdLink = By.Id("newAccountId");
        private readonly By errorMessage = By.XPath("//p[@class='error']");

        public LoanPage(IWebDriver driver) : base(driver) { }

        public void NavigateTo()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementToBeClickable(requestLoanLink));
            ClickElement(requestLoanLink);
        }

        public void RequestLoan(string amount, string downPayment, string fromAccountId)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(loanAmountField));
            
            SendKeys(loanAmountField, amount);
            SendKeys(downPaymentField, downPayment);
            
            // Handle AJAX dropdown
            wait.Until(d => new SelectElement(d.FindElement(fromAccountIdDropdown)).Options.Count > 0);
            var select = new SelectElement(driver.FindElement(fromAccountIdDropdown));
            select.SelectByValue(fromAccountId);
            
            ClickElement(applyNowButton);
        }

        public string GetLoanStatus()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(loanStatus));
            return GetText(loanStatus);
        }

        public string GetLoanResultMessage()
        {
            return GetText(loanResultMessage);
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

        public string GetPageErrorMessage()
        {
            try {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                wait.Until(ExpectedConditions.ElementIsVisible(errorMessage));
                return GetText(errorMessage);
            } catch {
                return string.Empty;
            }
        }
    }
}
