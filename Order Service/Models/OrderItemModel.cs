namespace Order_Service.Models
{
    public class OrderItemModel
    {
        public int ProductId { get; set; }
        public int OrderId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
