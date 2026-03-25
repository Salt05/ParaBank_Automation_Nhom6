using NUnit.Framework;
using ParaBank_Automation.Pages;
using ParaBank_Automation.Utilities;
using System;

namespace ParaBank_Automation.Tests
{
    [TestFixture]
    public class TransactionTests : BaseTest
    {
        private RegisterPage registerPage;
        private OpenNewAccountPage openNewAccountPage;
        private TransferPage transferPage;
        private BillPayPage billPayPage;
        private OverviewPage overviewPage;
        private LoginPage loginPage;

        // Đọc dữ liệu từ TestData
        private static readonly System.Collections.Generic.List<UserModel> _users =
            JsonHelper.ReadTestData<UserModel>("TestData/users.json");
        private static readonly TransferData _transferData =
            JsonHelper.ReadTestDataObject<TransferData>("TestData/transfer_data.json");
        private static readonly BillPayData _billPayData =
            JsonHelper.ReadTestDataObject<BillPayData>("TestData/billpay_data.json");

        private static UserModel JohnAccount => _users.Find(u => u.username == "john")!;

        [SetUp]
        public void TransactionSetUp()
        {
            registerPage = new RegisterPage(driver);
            openNewAccountPage = new OpenNewAccountPage(driver);
            transferPage = new TransferPage(driver);
            billPayPage = new BillPayPage(driver);
            overviewPage = new OverviewPage(driver);
            loginPage = new LoginPage(driver);
        }

        [Test]
        [Description("TC_F2.1_01: Chuyển tiền hợp lệ (Luồng chuẩn)")]
        public void TC_F2_1_01_TransferSuccess()
        {
            var user = JohnAccount;
            var data = _transferData.validTransfer;
            loginPage.Login(user.username, user.password);

            transferPage.NavigateToTransferFunds();
            string fromAccount = transferPage.GetFirstAvailableAccountId();
            string toAccount = transferPage.GetSecondAvailableAccountId();

            TestContext.WriteLine("--------------------------------------------------");
            TestContext.WriteLine($"👉 {data.description}");
            TestContext.WriteLine($"• Từ TK: {fromAccount} → Đến TK: {toAccount}");
            TestContext.WriteLine($"• Số tiền: ${data.amount}");
            TestContext.WriteLine("--------------------------------------------------");

            transferPage.SelectFromAccount(fromAccount);
            transferPage.SelectToAccount(toAccount);
            transferPage.EnterAmount(data.amount);
            transferPage.ClickTransfer();

            Assert.That(transferPage.GetTransferResultTitle(), Is.EqualTo("Transfer Complete!"), "Tiêu đề kết quả chuyển tiền không khớp!");
            Assert.That(transferPage.GetTransferResultMessage(), Contains.Substring($"${data.amount}.00 has been transferred"), "Nội dung kết quả chuyển tiền không đúng!");
        }

        [Test]
        [Description("TC_F2.1_02: Chuyển số tiền ÂM (Kiểm tra lỗi nghiệp vụ)")]
        public void TC_F2_1_02_TransferNegative()
        {
            var user = JohnAccount;
            var data = _transferData.negativeTransfer;
            loginPage.Login(user.username, user.password);

            transferPage.NavigateToTransferFunds();
            string fromAccount = transferPage.GetFirstAvailableAccountId();
            string toAccount = transferPage.GetSecondAvailableAccountId();

            overviewPage.NavigateTo();
            decimal balanceBefore = overviewPage.GetBalanceForAccount(fromAccount);

            TestContext.WriteLine("--------------------------------------------------");
            TestContext.WriteLine($"👉 {data.description}");
            TestContext.WriteLine($"• Tài khoản nguồn: {fromAccount}");
            TestContext.WriteLine($"• Số dư BAN ĐẦU: ${balanceBefore}");
            TestContext.WriteLine($"• Số tiền nhập: ${data.amount}");
            TestContext.WriteLine("--------------------------------------------------");

            transferPage.NavigateToTransferFunds();
            transferPage.SelectFromAccount(fromAccount);
            transferPage.SelectToAccount(toAccount);
            transferPage.EnterAmount(data.amount);
            transferPage.ClickTransfer();

            overviewPage.NavigateTo();
            decimal balanceAfter = overviewPage.GetBalanceForAccount(fromAccount);

            TestContext.WriteLine($"• Số dư SAU: ${balanceAfter}");
            TestContext.WriteLine("--------------------------------------------------");

            Assert.That(balanceAfter, Is.AtMost(balanceBefore), "BUG: Số dư bị tăng ngược (Hack tiền) khi chuyển số tiền âm!");
        }

        [Test]
        [Description("TC_F2.2_01: Thanh toán Bill & Trừ tiền")]
        public void TC_F2_2_01_BillPaySuccess()
        {
            var user = JohnAccount;
            var data = _billPayData.validPayment;
            loginPage.Login(user.username, user.password);

            overviewPage.NavigateTo();
            string primaryAccountId = overviewPage.GetFirstAccountId();
            decimal balanceBefore = overviewPage.CalculateTotalBalance();

            TestContext.WriteLine("--------------------------------------------------");
            TestContext.WriteLine($"👉 {data.description}");
            TestContext.WriteLine($"• Người nhận: {data.payeeName}");
            TestContext.WriteLine($"• Tài khoản nguồn: {primaryAccountId}");
            TestContext.WriteLine($"• Số dư BAN ĐẦU: ${balanceBefore} | Số tiền Bill: ${data.amount}");
            TestContext.WriteLine("--------------------------------------------------");

            billPayPage.NavigateToBillPay();
            billPayPage.FillBillPayForm(
                data.payeeName, data.address, data.city, data.state,
                data.zipCode, data.phone,
                data.accountNumber, data.verifyAccountNumber, data.amount
            );
            billPayPage.SelectFromAccount(primaryAccountId);
            billPayPage.ClickSendPayment();

            Assert.That(billPayPage.GetBillPayResultTitle(), Is.EqualTo("Bill Payment Complete"), "Tiêu đề thanh toán Bill không đúng!");

            overviewPage.NavigateTo();
            decimal balanceAfter = overviewPage.CalculateTotalBalance();
            decimal deducted = balanceBefore - balanceAfter;

            TestContext.WriteLine($"• Số dư SAU: ${balanceAfter} | Số tiền bị trừ: ${deducted}");
            TestContext.WriteLine("--------------------------------------------------");

            Assert.That(deducted, Is.EqualTo(decimal.Parse(data.amount)), "Số dư không được trừ chính xác sau thanh toán Bill!");
        }

        [Test]
        [Description("TC_F2.2_02: Verify Account không khớp")]
        public void TC_F2_2_02_BillPayMismatchedAccount()
        {
            var user = JohnAccount;
            var data = _billPayData.mismatchedPayment;
            loginPage.Login(user.username, user.password);

            TestContext.WriteLine("--------------------------------------------------");
            TestContext.WriteLine($"👉 {data.description}");
            TestContext.WriteLine($"• Nhập TK: {data.accountNumber} | Verify TK: {data.verifyAccountNumber}");
            TestContext.WriteLine("--------------------------------------------------");

            billPayPage.NavigateToBillPay();
            billPayPage.FillBillPayForm(
                data.payeeName, data.address, data.city, data.state,
                data.zipCode, data.phone,
                data.accountNumber, data.verifyAccountNumber, data.amount
            );
            billPayPage.ClickSendPayment();

            var error = billPayPage.GetVerifyAccountErrorMessage();
            Assert.That(error, Is.EqualTo("The account numbers do not match."), "Thông báo lỗi nhập sai account không đúng!");
        }
    }
}
