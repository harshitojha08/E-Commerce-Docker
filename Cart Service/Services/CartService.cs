using Cart_Service.Models;
using Newtonsoft.Json;

namespace Cart_Service.Services
{
    public class CartService
    {
        private readonly List<CartModel> _carts;
        private readonly List<CartItemModel> _cartItems;
        private readonly HttpClient _httpClient;

        public CartService(HttpClient httpClient)
        {
            _carts = new List<CartModel>();
            _cartItems = new List<CartItemModel>();
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task AddProductToCart(int userId, int productId, int quantity)
        {
            var cart = _carts.FirstOrDefault(c => c.UserId == userId);
            if (cart == null)
            {
                // Create a new cart for the user
                cart = new CartModel { UserId = userId, Id = _carts.Count + 1 };
                _carts.Add(cart);
            }

            var cartItem = new CartItemModel
            {
                Id = _cartItems.Count + 1,
                CartId = cart.Id,
                ProductId = productId,
                Quantity = quantity
            };
            _cartItems.Add(cartItem);
        }
        public async Task<List<ViewCartItemModel>> ViewCart(int userId)
        {
            int cartId = ExtractCartIdFromUserId(userId);

            if (cartId == 0)
            {
                return new List<ViewCartItemModel>();
            }

            List<CartItemModel> cartItems = GetCartItemsByCartId(cartId);

            List<ViewCartItemModel> viewCartItems = new List<ViewCartItemModel>();

            foreach (var cartItem in cartItems)
            {
                var productDetail = await GetProductByIdAsync(cartItem.ProductId);

                if (productDetail != null)
                {
                    var viewCartItem = MapToViewCartItemModel(cartItem, productDetail);
                    viewCartItems.Add(viewCartItem);
                }
            }

            return viewCartItems;
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

        private int ExtractCartIdFromUserId(int userId)
        {
            var cart = _carts.FirstOrDefault(c => c.UserId == userId);
            return cart?.Id ?? 0;
        }

        private List<CartItemModel> GetCartItemsByCartId(int cartId)
        {
            var userCartItems = _cartItems.Where(ci => ci.CartId == cartId).ToList();
            return userCartItems;
        }

        private ViewCartItemModel MapToViewCartItemModel(CartItemModel cartItem, ProductDetailModel productDetail)
        {
            var viewCartItem = new ViewCartItemModel
            {
                Id = cartItem.Id,
                ProductName = productDetail.Name,
                ImageUrl = productDetail.ImageUrl,
                Price = productDetail.Price,
                Quantity = cartItem.Quantity,
                TotalPrice = productDetail.Price * cartItem.Quantity
            };
            return viewCartItem;
        }

        public void EditCartItem(int userId, int cartItemId, int quantity)
        {
            var cartItem = _cartItems.FirstOrDefault(ci => ci.Id == cartItemId);
            if (cartItem == null || !_carts.Any(c => c.UserId == userId && c.Id == cartItem.CartId))
            {
                throw new InvalidOperationException("Cart item not found or does not belong to the user.");
            }

            cartItem.Quantity = quantity;
        }

        public void DeleteCartItem(int userId, int cartItemId)
        {
            var cartItem = _cartItems.FirstOrDefault(ci => ci.Id == cartItemId);
            if (cartItem == null || !_carts.Any(c => c.UserId == userId && c.Id == cartItem.CartId))
            {
                throw new InvalidOperationException("Cart item not found or does not belong to the user.");
            }

            _cartItems.Remove(cartItem);
        }
    }
}
