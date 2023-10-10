namespace SWP391_Group3_FinalProject.Models
{
    public class AdminAndStaff: Account
    {
        public string fullname { get; set; }
        public string email {  get; set; }
        public string SSN { get; set; }
        public string address { get; set;}
        public string phone { get; set; }
    }
}
