using Product_Details.Models;

namespace Product_Details.Services
{
    public class ProductService
    {
        private readonly List<ProductDetails> _products;

        public ProductService()
        {
            _products = new List<ProductDetails>();
        }

        public void AddProduct(int productId, string details)
        {
            if (_products.Any(p => p.ProductId == productId))
            {
                throw new ArgumentException("Product already exists.");
            }

            _products.Add(new ProductDetails { ProductId = productId, Details = details });
        }

        public void DeleteProduct(int productId)
        {
            var product = _products.FirstOrDefault(p => p.ProductId == productId);
            if (product != null)
            {
                _products.Remove(product);
            }
        }

        public ProductDetails GetProduct(int productId) 
        {
            var product = _products.FirstOrDefault(p => p.ProductId == productId);
            return product;
        }
    }
}
