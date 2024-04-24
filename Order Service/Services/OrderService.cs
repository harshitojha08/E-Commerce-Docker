using Newtonsoft.Json;
using Order_Service.Models;

namespace Order_Service.Services
{
    public class OrderService
    {
        private readonly List<OrderModel> _orders;
        private readonly HttpClient _httpClient;
        private readonly List<OrderItemModel> _orderItems;

        public OrderService(HttpClient httpClient)
        {
            _orders = new List<OrderModel>();
            _orderItems = new List<OrderItemModel>();
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<int> PlaceOrderAsync(int userId, List<OrderRequestItemDto> orderItems)
        {
            var order = new OrderModel
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                Id = _orders.Count + 1,
                OrderItems = new List<OrderItemModel>()
            };

            decimal finalTotal = 0;

            foreach (var item in orderItems)
            {
                var productDetail = await GetProductByIdAsync(item.ProductId);
                if (productDetail != null)
                {
                    decimal totalPrice = productDetail.Price * item.Quantity;
                    finalTotal += totalPrice;

                    order.OrderItems.Add(new OrderItemModel
                    {
                        OrderId = order.Id,
                        ProductId = item.ProductId,
                        ProductName = productDetail.Name,
                        Price = productDetail.Price,
                        ImageUrl = productDetail.ImageUrl,
                        Quantity = item.Quantity,
                        TotalPrice = totalPrice
                    });
                }
            }

            order.FinalTotal = finalTotal;
            _orders.Add(order);
            return order.Id;
        }

        public List<OrderModel> GetOrdersByUserId(int userId)
        {
            return _orders.Where(o => o.UserId == userId).ToList();
        }

        public OrderModel GetOrderById(int userId, int orderId)
        {
            return _orders.FirstOrDefault(o => o.UserId == userId && o.Id == orderId);
        }

        private async Task<ProductDetailModel> GetProductByIdAsync(int productId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Product/{productId}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ProductDetailModel>(content);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error fetching product details: {ex.Message}");
                return null;
            }
        }
        public void DeleteOrder(int userId, int orderId)
        {
            var orderItems = _orderItems.Where(oi => oi.OrderId == orderId).ToList();
            foreach (var orderItem in orderItems)
            {
                _orderItems.Remove(orderItem);
            }

            var order = _orders.FirstOrDefault(o => o.UserId == userId && o.Id == orderId);
            if (order != null)
            {
                _orders.Remove(order);
            }
            else
            {
                throw new KeyNotFoundException("Order not found.");
            }
        }
    }
}
