namespace Order_Service.Models
{
    public class ProductDetailModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public string Detail { get; set; }
    }
}
