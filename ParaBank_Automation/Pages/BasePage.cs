using OpenQA.Selenium;

namespace ParaBank_Automation.Pages
{
    // Lớp cha cho tất cả các Page, chứa driver và các hàm dùng chung
    public abstract class BasePage
    {
        protected IWebDriver driver;

        public BasePage(IWebDriver webDriver)
        {
            driver = webDriver;
        }

        // Click vào element theo locator
        protected void ClickElement(By locator)
        {
            driver.FindElement(locator).Click();
        }

        // Gửi text vào element theo locator
        protected void SendKeys(By locator, string text)
        {
            driver.FindElement(locator).Clear();
            driver.FindElement(locator).SendKeys(text);
        }

        // Lấy text của element theo locator
        public string GetText(By locator)
        {
            return driver.FindElement(locator).Text;
        }
    }
}
