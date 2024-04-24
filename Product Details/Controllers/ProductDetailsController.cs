using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Product_Details.Models;
using Product_Details.Services;

namespace Product_Details.Controllers
{
    
    [ApiController]
    [Route("api/details")]
    public class ProductDetailsController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductDetailsController(ProductService productService)
        {
            _productService = productService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("add")]
        public IActionResult AddProduct([FromBody] ProductDetails productDetails)
        {
            try
            {
                _productService.AddProduct(productDetails.ProductId, productDetails.Details);
                return Ok("Product added successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{productId}")]
        public IActionResult DeleteProduct(int productId)
        {
            try
            {
                _productService.DeleteProduct(productId);
                return Ok("Product deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{productId}")]
        public IActionResult GetProduct(int productId)
        {
             var product = _productService.GetProduct(productId);
             if (product == null)
             {
                 return NotFound();
             }
                 return Ok(product);
        }
    }
}
