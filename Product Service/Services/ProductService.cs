using Newtonsoft.Json;
using Product_Service.Models;

namespace Product_Service.Service
{
    public class ProductService
    {
        private readonly List<ProductModel> products;
        private readonly HttpClient _httpClient;

        public ProductService(HttpClient httpClient)
        {
            products = new List<ProductModel>();
            AddDummyData();
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public void AddProduct(ProductModel product)
        {
            product.Id = products.Count + 1;
            products.Add(product);
        }

        public void DeleteProduct(int productId)
        {
            products.RemoveAll(p => p.Id == productId);
        }

        public void UpdateProduct(ProductModel updatedProduct)
        {
            int index = products.FindIndex(p => p.Id == updatedProduct.Id);
            if (index != -1)
            {
                products[index] = updatedProduct;
            }
            else
            {
                throw new KeyNotFoundException($"Product with ID {updatedProduct.Id} not found.");
            }
        }

        public List<ProductModel> GetAllProducts()
        {
            List<ProductModel> summaries = new List<ProductModel>();
            foreach (var product in products)
            {
                summaries.Add(new ProductModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    ImageUrl = product.ImageUrl
                });
            }
            return summaries;
        }

        public async Task<ProductDetailsModel> GetProductById(int productId)
        {
            var tempProduct = products.Find(p => p.Id == productId);
            var detail = await GetProductByIdAsync(productId);
            if (detail!=null)
            {
                var product1 = new ProductDetailsModel
                {
                    Id = detail.ProdutId,
                    Name = tempProduct.Name,
                    Description = tempProduct.Description,
                    Price = tempProduct.Price,
                    ImageUrl = tempProduct.ImageUrl,
                    Details = detail.Details
                };
                return product1;
            }
            var product = new ProductDetailsModel
            {
                Id = tempProduct.Id,
                Name = tempProduct.Name,
                Description = tempProduct.Description,
                Price = tempProduct.Price,
                ImageUrl = tempProduct.ImageUrl,
            };
            return product;
        }

        private async Task<ProductSummaryModel> GetProductByIdAsync(int productId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/details/get/{productId}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ProductSummaryModel>(content);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error fetching product details: {ex.Message}");
                return null;
            }
        }

        private void AddDummyData()
        {
            AddProduct(new ProductModel
            {
                Id = 1,
                Name = "T-Shirt",
                Description = "Men's Cotton T-Shirt",
                Price = 199,
                ImageUrl = "https://example.com/tshirt.jpg",
            });

            AddProduct(new ProductModel
            {
                Id = 2,
                Name = "Smartphone",
                Description = "Latest Smartphone",
                Price = 599,
                ImageUrl = "https://example.com/smartphone.jpg",
            });
        }
    }
}
