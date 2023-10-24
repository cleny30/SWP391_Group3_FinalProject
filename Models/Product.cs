namespace SWP391_Group3_FinalProject.Models
{
    public class Product
    {
        public string pro_id { get; set; }
        public int brand_id { get; set; }
        public int cate_id { get; set; }
        public string pro_name { get; set; }
        public int pro_quan { get; set; }
        public string pro_des { get; set; }
        public double pro_price { get; set; }
        public int discount { get; set; }
        public bool isAvailable { get; set; }
        public List<string> pro_img { get; set; } = new List<string>();
        public Dictionary<string, string> pro_attribute { get; set; } = new Dictionary<string, string>();
    }
}
