namespace ParaBank_Automation.Utilities
{
    // Model mapping đúng cấu trúc users.json
    public class UserModel
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zipCode { get; set; }
        public string phone { get; set; }
        public string ssn { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string confirmPassword { get; set; }
    }
}
