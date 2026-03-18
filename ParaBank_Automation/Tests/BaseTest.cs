using NUnit.Framework;
using OpenQA.Selenium;
using ParaBank_Automation.Utilities;

namespace ParaBank_Automation.Tests
{
    // Lớp base cho các test, khởi tạo và đóng driver
    public class BaseTest
    {
        protected IWebDriver? driver;

        [SetUp]
        public void SetUp()
        {
            driver = DriverFactory.GetDriver();
            driver.Navigate().GoToUrl("https://parabank.parasoft.com/parabank/index.htm");
        }

        [TearDown]
        public void TearDown()
        {
            DriverFactory.QuitDriver();
            if (driver != null)
            {
                driver.Dispose();
                driver = null;
            }
        }
    }
}
