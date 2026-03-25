using System.Text.Json.Serialization;

namespace ParaBank_Automation.Utilities
{
    // ===== TRANSFER DATA MODELS =====
    public class TransferScenario
    {
        [JsonPropertyName("amount")] public string amount { get; set; } = "";
        [JsonPropertyName("description")] public string description { get; set; } = "";
    }

    public class SpamTransferScenario : TransferScenario
    {
        [JsonPropertyName("spamClickCount")] public int spamClickCount { get; set; } = 5;
    }

    public class TransferData
    {
        [JsonPropertyName("validTransfer")] public TransferScenario validTransfer { get; set; } = new();
        [JsonPropertyName("negativeTransfer")] public TransferScenario negativeTransfer { get; set; } = new();
        [JsonPropertyName("spamTransfer")] public SpamTransferScenario spamTransfer { get; set; } = new();
        [JsonPropertyName("overflowTransfer")] public TransferScenario overflowTransfer { get; set; } = new();
    }

    // ===== BILL PAY DATA MODELS =====
    public class BillPayScenario
    {
        [JsonPropertyName("payeeName")] public string payeeName { get; set; } = "";
        [JsonPropertyName("address")] public string address { get; set; } = "";
        [JsonPropertyName("city")] public string city { get; set; } = "";
        [JsonPropertyName("state")] public string state { get; set; } = "";
        [JsonPropertyName("zipCode")] public string zipCode { get; set; } = "";
        [JsonPropertyName("phone")] public string phone { get; set; } = "";
        [JsonPropertyName("accountNumber")] public string accountNumber { get; set; } = "";
        [JsonPropertyName("verifyAccountNumber")] public string verifyAccountNumber { get; set; } = "";
        [JsonPropertyName("amount")] public string amount { get; set; } = "";
        [JsonPropertyName("description")] public string description { get; set; } = "";
    }

    public class BillPayData
    {
        [JsonPropertyName("validPayment")] public BillPayScenario validPayment { get; set; } = new();
        [JsonPropertyName("mismatchedPayment")] public BillPayScenario mismatchedPayment { get; set; } = new();
    }

    // ===== ACCOUNT DATA MODELS =====
    public class AccountScenario
    {
        [JsonPropertyName("accountType")] public string accountType { get; set; } = "";
        [JsonPropertyName("description")] public string description { get; set; } = "";
    }

    public class SeedTransferScenario
    {
        [JsonPropertyName("amount")] public string amount { get; set; } = "";
        [JsonPropertyName("description")] public string description { get; set; } = "";
    }

    public class LoanScenario
    {
        [JsonPropertyName("loanAmount")] public string loanAmount { get; set; } = "";
        [JsonPropertyName("downPayment")] public string downPayment { get; set; } = "";
        [JsonPropertyName("description")] public string description { get; set; } = "";
    }

    public class AccountData
    {
        [JsonPropertyName("openChecking")] public AccountScenario openChecking { get; set; } = new();
        [JsonPropertyName("openSavings")] public AccountScenario openSavings { get; set; } = new();
        [JsonPropertyName("seedTransfer")] public SeedTransferScenario seedTransfer { get; set; } = new();
        [JsonPropertyName("approvedLoan")] public LoanScenario approvedLoan { get; set; } = new();
        [JsonPropertyName("insufficientLoan")] public LoanScenario insufficientLoan { get; set; } = new();
    }

    // ===== SECURITY DATA MODELS =====
    public class ProfileUpdateScenario
    {
        [JsonPropertyName("city")] public string city { get; set; } = "";
        [JsonPropertyName("phone")] public string phone { get; set; } = "";
        [JsonPropertyName("description")] public string description { get; set; } = "";
    }

    public class DuplicateUserScenario
    {
        [JsonPropertyName("firstName")] public string firstName { get; set; } = "";
        [JsonPropertyName("lastName")] public string lastName { get; set; } = "";
        [JsonPropertyName("address")] public string address { get; set; } = "";
        [JsonPropertyName("city")] public string city { get; set; } = "";
        [JsonPropertyName("state")] public string state { get; set; } = "";
        [JsonPropertyName("zipCode")] public string zipCode { get; set; } = "";
        [JsonPropertyName("phone")] public string phone { get; set; } = "";
        [JsonPropertyName("username")] public string username { get; set; } = "";
        [JsonPropertyName("password")] public string password { get; set; } = "";
        [JsonPropertyName("confirmPassword")] public string confirmPassword { get; set; } = "";
        [JsonPropertyName("description")] public string description { get; set; } = "";
    }

    public class LoginFailScenario
    {
        [JsonPropertyName("username")] public string username { get; set; } = "";
        [JsonPropertyName("wrongPassword")] public string wrongPassword { get; set; } = "";
        [JsonPropertyName("description")] public string description { get; set; } = "";
    }

    public class SecurityData
    {
        [JsonPropertyName("profileUpdate")] public ProfileUpdateScenario profileUpdate { get; set; } = new();
        [JsonPropertyName("xssPayload")] public ProfileUpdateScenario xssPayload { get; set; } = new();
        [JsonPropertyName("loginFail")] public LoginFailScenario loginFail { get; set; } = new();
        [JsonPropertyName("duplicateUser")] public DuplicateUserScenario duplicateUser { get; set; } = new();
    }
}
