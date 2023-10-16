namespace SWP391_Group3_FinalProject.Models
{
    public class OrderDetail
    {
        public string orderID {  get; set; }
        public string productID { get; set; }
        public string username { get; set; }
        public string productName {  get; set; }
        public int quantity { get; set; }
        public double price { get; set; }
    }
}
