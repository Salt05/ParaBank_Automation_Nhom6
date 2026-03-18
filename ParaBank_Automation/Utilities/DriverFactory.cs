using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ParaBank_Automation.Utilities
{
    public static class DriverFactory
    {
        private static IWebDriver driver;

        // Lấy hoặc khởi tạo driver
        public static IWebDriver GetDriver()
        {
            if (driver == null)
            {
                driver = new ChromeDriver();
                driver.Manage().Window.Maximize();
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            }
            return driver;
        }

        // Đóng trình duyệt
        public static void QuitDriver()
        {
            if (driver != null)
            {
                driver.Quit();
                driver = null;
            }
        }
    }
}