using Cart_Service.Models;
using Cart_Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Cart_Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly CartService _cartService;

        public CartController(CartService cartService)
        {
            _cartService = cartService;
        }
 
        [HttpPost("add")]
        [Authorize]
        public IActionResult AddToCart([FromBody] AddCartItemDto cartItemDto)
        {
            var userIdClaim = User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            int userId = Convert.ToInt32(userIdClaim.Value);

            if (userId==null)
            {
                return Unauthorized();
            }

            _cartService.AddProductToCart(userId, cartItemDto.ProductId, cartItemDto.Quantity);
            return Ok("Product added to cart successfully.");
        }

        [HttpGet("view")]
        public async Task<IActionResult> ViewCart()
        {
            var userIdClaim = User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            int userId = Convert.ToInt32(userIdClaim.Value);
            try
            {
                var viewCartItems = await _cartService.ViewCart(userId);
                return Ok(viewCartItems);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{cartItemId}")]
        [Authorize]
        public IActionResult UpdateCartItem(int cartItemId, [FromBody] CartItemModel cartItemDto)
        {
            var userIdClaim = User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            int userId = Convert.ToInt32(userIdClaim.Value);

            if (userId == null)
            {
                return Unauthorized();
            }

            _cartService.EditCartItem(userId, cartItemId, cartItemDto.Quantity);
            return Ok("Product added to cart successfully.");
        }

        [HttpDelete("{cartItemId}")]
        [Authorize]
        public IActionResult DeleteCartItem(int cartItemId)
        {
            var userIdClaim = User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            int userId = Convert.ToInt32(userIdClaim.Value);

            if (userId == null)
            {
                return Unauthorized();
            }

            _cartService.DeleteCartItem(userId, cartItemId);
            return Ok("Cart item deleted successfully.");
        }
    }
}
