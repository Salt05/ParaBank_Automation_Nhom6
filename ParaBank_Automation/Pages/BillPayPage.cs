using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace ParaBank_Automation.Pages
{
    public class BillPayPage : BasePage
    {
        // ===== LOCATORS =====
        private readonly By billPayLink = By.LinkText("Bill Pay");
        private readonly By payeeNameField = By.Name("payee.name");
        private readonly By addressField = By.Name("payee.address.street");
        private readonly By cityField = By.Name("payee.address.city");
        private readonly By stateField = By.Name("payee.address.state");
        private readonly By zipCodeField = By.Name("payee.address.zipCode");
        private readonly By phoneNumberField = By.Name("payee.phoneNumber");
        private readonly By accountNumberField = By.Name("payee.accountNumber");
        private readonly By verifyAccountField = By.Name("verifyAccount");
        private readonly By amountField = By.Name("amount");
        private readonly By fromAccountIdDropdown = By.Name("fromAccountId");
        private readonly By sendPaymentButton = By.XPath("//input[@value='Send Payment']");
        private readonly By billPayResultTitle = By.XPath("//div[@id='billpayResult']/h1");
        private readonly By billPayResultMessage = By.XPath("//div[@id='billpayResult']/p");

        // Error message locators (can be multiple)
        private readonly By payeeNameError = By.XPath("//span[@ng-show='billPayForm.payee.name.$invalid']");
        private readonly By verifyAccountError = By.XPath("//span[text()='The account numbers do not match.']");
        private readonly By amountError = By.XPath("//span[@ng-show='billPayForm.amount.$invalid']");

        public BillPayPage(IWebDriver driver) : base(driver) { }

        public void NavigateToBillPay()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementToBeClickable(billPayLink));
            ClickElement(billPayLink);
        }

        public void EnterPayeeName(string name) => SendKeys(payeeNameField, name);
        public void EnterAddress(string address) => SendKeys(addressField, address);
        public void EnterCity(string city) => SendKeys(cityField, city);
        public void EnterState(string state) => SendKeys(stateField, state);
        public void EnterZipCode(string zipCode) => SendKeys(zipCodeField, zipCode);
        public void EnterPhoneNumber(string phone) => SendKeys(phoneNumberField, phone);
        public void EnterAccountNumber(string accountNum) => SendKeys(accountNumberField, accountNum);
        public void EnterVerifyAccount(string verifyAccountNum) => SendKeys(verifyAccountField, verifyAccountNum);
        public void EnterAmount(string amount) => SendKeys(amountField, amount);

        public void SelectFromAccount(string accountId)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => new SelectElement(d.FindElement(fromAccountIdDropdown)).Options.Count > 0);
            var select = new SelectElement(driver.FindElement(fromAccountIdDropdown));
            select.SelectByValue(accountId);
        }

        public void ClickSendPayment()
        {
            ClickElement(sendPaymentButton);
        }

        public string GetBillPayResultTitle()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(billPayResultTitle));
            return GetText(billPayResultTitle);
        }

        public string GetBillPayResultMessage()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(billPayResultMessage));
            return GetText(billPayResultMessage);
        }

        public string GetPayeeNameErrorMessage()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(payeeNameError));
            return GetText(payeeNameError);
        }

        public string GetVerifyAccountErrorMessage()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(verifyAccountError));
            return GetText(verifyAccountError);
        }

        public string GetAmountErrorMessage()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(amountError));
            return GetText(amountError);
        }

        public void FillBillPayForm(string payeeName, string street, string city, string state, string zip, string phone, string account, string verifyAccount, string amount)
        {
            EnterPayeeName(payeeName);
            EnterAddress(street);
            EnterCity(city);
            EnterState(state);
            EnterZipCode(zip);
            EnterPhoneNumber(phone);
            EnterAccountNumber(account);
            EnterVerifyAccount(verifyAccount);
            EnterAmount(amount);
        }
    }
}
