using MauzoHub.MVCApp.Models;

namespace MauzoHub.MVCApp
{
    public class ProductService
    {
        private readonly HttpClient _httpClient;
        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7251/api/");
        }

        public async Task CreateProduct(Product product)
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
            new KeyValuePair<string, string>("Name", product.Name),
            new KeyValuePair<string, string>("Description", product.Description)
            // If you have additional properties, add them here as well.
        });

            await _httpClient.PostAsync("Products", formContent);
        }

        public async Task<List<Product>> GetProducts()
        {
            var response = await _httpClient.GetFromJsonAsync<List<Product>>("Products");
            return response;
        }

        public async Task<Product> GetProductById(Guid id)
        {
            var response = await _httpClient.GetFromJsonAsync<Product>($"Products/id/{id}");
            return response;
        }

        public async Task UpdateProduct(Product product)
        {
            await _httpClient.PutAsJsonAsync($"Products/update", product);
        }

        public async Task DeleteProduct(Guid id)
        {
            await _httpClient.DeleteAsync($"Products/id/{id}");
        }
    }
}
