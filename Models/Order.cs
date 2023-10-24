namespace SWP391_Group3_FinalProject.Models
{
    public class Order
    {
        public string orderId {  get; set; }
        public string? staffId { get; set; }
        public string username { get; set; }
        public double totalPrice { get; set; }
        public DateTime startDay { get; set; }
        public DateTime? endDay { get; set; }
        public string? description { get; set; }
        public int status { get; set; }
    }
}
