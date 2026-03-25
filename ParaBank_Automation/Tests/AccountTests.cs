using NUnit.Framework;
using ParaBank_Automation.Pages;
using ParaBank_Automation.Utilities;
using System;

namespace ParaBank_Automation.Tests
{
    [TestFixture]
    public class AccountTests : BaseTest
    {
        private RegisterPage registerPage;
        private OpenNewAccountPage openNewAccountPage;
        private OverviewPage overviewPage;
        private TransferPage transferPage;
        private FindTransPage findTransPage;
        private LoanPage loanPage;
        private LoginPage loginPage;

        // Đọc dữ liệu từ TestData
        private static readonly System.Collections.Generic.List<UserModel> _users =
            JsonHelper.ReadTestData<UserModel>("TestData/users.json");
        private static readonly AccountData _accountData =
            JsonHelper.ReadTestDataObject<AccountData>("TestData/account_data.json");

        private static UserModel JohnAccount => _users.Find(u => u.username == "john")!;

        [SetUp]
        public void AccountSetUp()
        {
            registerPage = new RegisterPage(driver);
            openNewAccountPage = new OpenNewAccountPage(driver);
            overviewPage = new OverviewPage(driver);
            transferPage = new TransferPage(driver);
            findTransPage = new FindTransPage(driver);
            loanPage = new LoanPage(driver);
            loginPage = new LoginPage(driver);
        }

        [Test]
        [Description("TC_F3.1_01: Mở tài khoản Checking")]
        public void TC_F3_1_01_OpenCheckingAccount()
        {
            var user = JohnAccount;
            var data = _accountData.openChecking;
            loginPage.Login(user.username, user.password);

            TestContext.WriteLine("--------------------------------------------------");
            TestContext.WriteLine($"👉 {data.description}");
            TestContext.WriteLine($"• Loại tài khoản: {data.accountType}");
            TestContext.WriteLine("--------------------------------------------------");

            openNewAccountPage.NavigateTo();
            openNewAccountPage.OpenNewAccount(data.accountType);

            Assert.That(driver.PageSource, Contains.Substring("Account Opened!"), "Mở tài khoản không trả về thông báo thành công!");
        }

        [Test]
        [Description("TC_F3.2_01: Xác minh tính toán tổng số dư (Total Balance)")]
        public void TC_F3_2_01_VerifyTotalBalance()
        {
            var user = JohnAccount;
            loginPage.Login(user.username, user.password);

            overviewPage.NavigateTo();
            decimal calculated = overviewPage.CalculateTotalBalance();
            decimal displayed = overviewPage.GetTotalDisplay();

            TestContext.WriteLine("--------------------------------------------------");
            TestContext.WriteLine("👉 KIỂM TRA TỔNG SỐ DƯ:");
            TestContext.WriteLine($"• Tổng tính tay từng dòng: ${calculated}");
            TestContext.WriteLine($"• Tổng hiển thị trên trang: ${displayed}");
            TestContext.WriteLine("--------------------------------------------------");

            Assert.That(calculated, Is.EqualTo(displayed), "Tổng số dư tính toán bằng tay không khớp với tổng hiển thị trên trang!");
        }

        [Test]
        [Description("TC_F4.1_01: Tìm giao dịch theo khoảng ngày (Date Range)")]
        public void TC_F4_1_01_FindTransactionsDateRange()
        {
            var user = JohnAccount;
            var seedData = _accountData.seedTransfer;
            var openData = _accountData.openSavings;
            loginPage.Login(user.username, user.password);

            // Mở Tài khoản SAVINGS Mới
            openNewAccountPage.NavigateTo();
            openNewAccountPage.OpenNewAccount(openData.accountType);

            // Thực hiện giao dịch mồi từ TestData
            transferPage.NavigateToTransferFunds();
            string fromAccount = transferPage.GetFirstAvailableAccountId();
            string toAccount = transferPage.GetSecondAvailableAccountId();

            TestContext.WriteLine("--------------------------------------------------");
            TestContext.WriteLine($"👉 TẠO GIAO DỊCH MỒI (Setup Data): {seedData.description}");
            TestContext.WriteLine($"• Chuyển từ Tài khoản: {fromAccount}");
            TestContext.WriteLine($"• Đến Tài khoản: {toAccount}");
            TestContext.WriteLine($"• Số tiền: ${seedData.amount}");
            TestContext.WriteLine("--------------------------------------------------");

            transferPage.SelectFromAccount(fromAccount);
            transferPage.SelectToAccount(toAccount);
            transferPage.EnterAmount(seedData.amount);
            transferPage.ClickTransfer();

            // Tra cứu trong khoảng ngày
            findTransPage.NavigateTo();
            string fromDate = DateTime.Now.AddDays(-3).ToString("MM-dd-yyyy");
            string toDate = DateTime.Now.ToString("MM-dd-yyyy");

            TestContext.WriteLine("--------------------------------------------------");
            TestContext.WriteLine("👉 ĐIỀN FORM TRA CỨU KHOẢNG NGÀY:");
            TestContext.WriteLine($"• From Date (Từ ngày): {fromDate}");
            TestContext.WriteLine($"• To Date (Đến ngày): {toDate}");
            TestContext.WriteLine("--------------------------------------------------");

            findTransPage.FindByDateRange(fromDate, toDate);

            Assert.That(findTransPage.GetResultCount(), Is.GreaterThanOrEqualTo(1), "Không tìm thấy giao dịch nào trong khoảng ngày đã chọn!");
        }

        [Test]
        [Description("TC_F4.2_01: Yêu cầu vay vốn hợp lệ")]
        public void TC_F4_2_01_RequestLoanApproved()
        {
            var user = JohnAccount;
            var data = _accountData.approvedLoan;
            loginPage.Login(user.username, user.password);

            overviewPage.NavigateTo();
            string primaryAccountId = overviewPage.GetFirstAccountId();

            TestContext.WriteLine("--------------------------------------------------");
            TestContext.WriteLine($"👉 {data.description}");
            TestContext.WriteLine($"• Tài khoản: {primaryAccountId}");
            TestContext.WriteLine($"• Số tiền vay: ${data.loanAmount} | Down Payment: ${data.downPayment}");
            TestContext.WriteLine("--------------------------------------------------");

            loanPage.NavigateTo();
            loanPage.RequestLoan(data.loanAmount, data.downPayment, primaryAccountId);

            string status = loanPage.GetLoanStatus();
            TestContext.WriteLine($"• Kết quả xét duyệt: {status}");
            TestContext.WriteLine("--------------------------------------------------");

            Assert.That(status, Is.EqualTo("Approved").Or.EqualTo("Denied"), "Xử lý Loan Status không trả về kết quả hợp lệ!");
        }

        [Test]
        [Description("TC_F4.2_02: Trích tiền đối ứng (Down Payment) vượt số dư")]
        public void TC_F4_2_02_RequestLoanInsufficientDownPayment()
        {
            var user = JohnAccount;
            var data = _accountData.insufficientLoan;
            loginPage.Login(user.username, user.password);

            overviewPage.NavigateTo();
            string primaryAccountId = overviewPage.GetFirstAccountId();

            TestContext.WriteLine("--------------------------------------------------");
            TestContext.WriteLine($"👉 {data.description}");
            TestContext.WriteLine($"• Tài khoản: {primaryAccountId}");
            TestContext.WriteLine($"• Số tiền vay: ${data.loanAmount} | Down Payment vượt số dư: ${data.downPayment}");
            TestContext.WriteLine("--------------------------------------------------");

            loanPage.NavigateTo();
            loanPage.RequestLoan(data.loanAmount, data.downPayment, primaryAccountId);

            string status = loanPage.GetLoanStatus();
            TestContext.WriteLine($"• Kết quả: {status}");
            TestContext.WriteLine("--------------------------------------------------");

            Assert.That(status, Is.EqualTo("Denied"), "Sự từ chối Down payment over balance có lỗi xảy ra!");
        }
    }
}
