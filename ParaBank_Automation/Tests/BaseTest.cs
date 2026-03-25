using NUnit.Framework;
using OpenQA.Selenium;
using ParaBank_Automation.Utilities;
using System;
using System.IO;

namespace ParaBank_Automation.Tests
{
    // Lớp base cho các test, khởi tạo và đóng driver
    public class BaseTest
    {
        protected IWebDriver? driver;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            try
            {
                using (var client = new System.Net.Http.HttpClient())
                {
                    var cleanResponse = client.PostAsync("https://parabank.parasoft.com/parabank/services/util/cleanDatabase", null).Result;
                    var initResponse = client.PostAsync("https://parabank.parasoft.com/parabank/services/util/initializeDatabase", null).Result;
                    if (!cleanResponse.IsSuccessStatusCode || !initResponse.IsSuccessStatusCode)
                    {
                        TestContext.WriteLine("Cảnh báo: Không thể reset database hoàn toàn từ API.");
                    }
                }
            }
            catch (Exception ex)
            {
                TestContext.WriteLine($"Lỗi khi gọi API Clean Database: {ex.Message}");
            }
        }

        /// <summary>
        /// Hàm chụp màn hình phục vụ Evidence-based Testing.
        /// </summary>
        /// <param name="testCaseId">ID Test Case (VD: TC_F1.1_01)</param>
        /// <param name="stepName">Tên bước (VD: Created_First_User)</param>
        public string TakeScreenshot(string testCaseId, string stepName)
        {
            if (driver == null) return string.Empty;
            try
            {
                string dateStr = DateTime.Now.ToString("yyyy-MM-dd");
                // Lưu vào Reports/Screenshots/{ngay}
                string screenshotsDir = Path.Combine(Directory.GetCurrentDirectory(), "ParaBank_Automation", "Reports", "Screenshots", dateStr);
                
                if (!Directory.Exists(screenshotsDir))
                    Directory.CreateDirectory(screenshotsDir);

                string timestamp = DateTime.Now.ToString("HHmmss");
                // Tên file {testCaseId}_{stepName}_{HHmmss}.png
                string fileName = $"{testCaseId}_{stepName}_{timestamp}.png";
                string filePath = Path.Combine(screenshotsDir, fileName);

                var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                screenshot.SaveAsFile(filePath);
                return filePath; // Trả về đường dẫn để gắn vào báo cáo
            }
            catch (Exception ex)
            {
                TestContext.WriteLine($"Không thể chụp screenshot cho {testCaseId} - {stepName}: {ex.Message}");
                return string.Empty;
            }
        }

        [SetUp]
        public void SetUp()
        {
            driver = DriverFactory.GetDriver();
            
            // LUẬT LỆ TỐI THƯỢNG: Reset môi trường qua trang Admin trước mỗi Test Case
            try
            {
                var adminPage = new Pages.AdminPage(driver);
                adminPage.NavigateTo();
                
                adminPage.ClickCleanDatabase();
                TakeScreenshot("Setup", "CleanDB_Success");
                
                adminPage.ClickInitializeDatabase();
                TakeScreenshot("Setup", "InitDB_Success");
            }
            catch (Exception ex)
            {
                 TestContext.WriteLine($"Lỗi khi thiết lập môi trường trong SetUp: {ex.Message}");
            }

            // Xóa toàn bộ Cookie cũ → Đảm bảo Session sạch trước mỗi Test
            driver.Manage().Cookies.DeleteAllCookies();
            driver.Navigate().GoToUrl("https://parabank.parasoft.com/parabank/index.htm");
        }

        [TearDown]
        public void TearDown()
        {
            var outcome = TestContext.CurrentContext.Result.Outcome.Status;
            var testName = TestContext.CurrentContext.Test.Name;
            
            // Trích xuất TestCaseId từ testName nếu định dạng cho phép
            string testCaseId = testName.Contains("_") ? testName.Split('_')[0] : "TearDown";

            if (driver != null)
            {
                // Chụp màn hình nếu kiểm thử THẤT BẠI
                if (outcome == NUnit.Framework.Interfaces.TestStatus.Failed)
                {
                    string screenshotPath = TakeScreenshot(testCaseId, "Failed_Screenshot");
                    
                    // Ghi Báo cáo lỗi vào Excel
                    ExcelReportHelper.LogFailedTest(testCaseId, screenshotPath);
                    TestContext.WriteLine($"👉 Đã chụp & ghi Excel ảnh lỗi cho {testCaseId}");
                }

                // LUẬT LỆ TỐI THƯỢNG: Clean Database sau khi test xong
                try
                {
                    var adminPage = new Pages.AdminPage(driver);
                    adminPage.NavigateTo();
                    adminPage.ClickCleanDatabase();
                    TakeScreenshot(testCaseId, "TearDown_CleanDB_Success");
                }
                catch (Exception ex)
                {
                    TestContext.WriteLine($"Không thể Clean Database trong TearDown: {ex.Message}");
                }
            }

            DriverFactory.QuitDriver();
            if (driver != null)
            {
                driver.Dispose();
                driver = null;
            }
        }
    }
}
