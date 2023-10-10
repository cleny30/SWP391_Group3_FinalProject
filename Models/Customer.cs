namespace SWP391_Group3_FinalProject.Models
{
    public class Customer: Account
    {
        public string fullname { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public List<Addresses> addresses { get; set; }
    }
}
