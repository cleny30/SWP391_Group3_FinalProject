namespace SWP391_Group3_FinalProject.Models
{
    public class Customer : Account
    {
        public List<Addresses> addresses { get; set; } = new List<Addresses>();
    }
}
