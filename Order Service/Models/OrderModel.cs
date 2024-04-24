namespace Order_Service.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItemModel> OrderItems { get; set; }
        public decimal FinalTotal { get; set; }
    }
}
