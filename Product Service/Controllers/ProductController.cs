using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Product_Service.Models;
using Product_Service.Service;

namespace Product_Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService productService;

        public ProductController(ProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products =  productService.GetAllProducts();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult AddProduct(ProductModel product)
        {
            productService.AddProduct(product);
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateProduct(int id, ProductModel product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            productService.UpdateProduct(product);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteProduct(int id)
        {
            productService.DeleteProduct(id);
            return NoContent();
        }

    }
}
