using OfficeOpenXml;
using System;
using System.IO;

namespace ParaBank_Automation.Utilities
{
    public static class ExcelReportHelper
    {
        // Tạo Repoert trong thư mục bin hoặc thư mục gốc
        private static readonly string reportPath = Path.Combine(Directory.GetCurrentDirectory(), "Test_Execution_Report.xlsx");

        public static void InitializeExcel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            
            if (!File.Exists(reportPath))
            {
                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Failed Tests");
                    worksheet.Cells[1, 1].Value = "Test Case ID";
                    worksheet.Cells[1, 2].Value = "Thời gian Chạy";
                    worksheet.Cells[1, 3].Value = "Hình ảnh minh họa (Screenshot)";

                    worksheet.Cells[1, 1, 1, 3].Style.Font.Bold = true;
                    // Auto-fit Column widths
                    worksheet.Column(1).Width = 20;
                    worksheet.Column(2).Width = 20;
                    worksheet.Column(3).Width = 50;

                    package.SaveAs(new FileInfo(reportPath));
                }
            }
        }

        public static void LogFailedTest(string testCaseId, string screenshotPath)
        {
            InitializeExcel();

            using (var package = new ExcelPackage(new FileInfo(reportPath)))
            {
                var worksheet = package.Workbook.Worksheets["Failed Tests"];
                int row = worksheet.Dimension != null ? worksheet.Dimension.End.Row + 1 : 2;

                worksheet.Cells[row, 1].Value = testCaseId;
                worksheet.Cells[row, 2].Value = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");

                if (File.Exists(screenshotPath))
                {
                    worksheet.Row(row).Height = 150; // Tăng chiều cao để chứa ảnh
                    var image = worksheet.Drawings.AddPicture(testCaseId + "_" + row, new FileInfo(screenshotPath));
                    
                    // Định vị Ảnh khớp với Cột 3 (Index 2)
                    image.SetPosition(row - 1, 0, 2, 0); 
                    image.SetSize(300, 150); // Set Picture Size
                }
                else
                {
                    worksheet.Cells[row, 3].Value = "Không nạp được file Screenshot!";
                }

                package.Save();
            }
        }
    }
}
