namespace SWP391_Group3_FinalProject.Models
{
    public class Manager : Account
    {

        public int ID { get; set; }
        public string SSN { get; set; }
        public string address { get; set; }
        public bool isAdmin { get; set; }
    }
}
