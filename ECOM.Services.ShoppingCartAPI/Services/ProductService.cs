using ECOM.Services.ShoppingCartAPI.Models.DTO;
using ECOM.Services.ShoppingCartAPI.Services.IServices;
using Newtonsoft.Json;

namespace ECOM.Services.ShoppingCartAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ProductService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<ProductDTO>> GetProducts()
        {
            var client = _httpClientFactory.CreateClient("Product");
            var response = await client.GetAsync($"/Get");
            var apicontent = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonConvert.DeserializeObject<ResponseDTO>(apicontent);

            if (jsonResponse != null && jsonResponse.IsSuccess) { return JsonConvert.DeserializeObject<IEnumerable<ProductDTO>>(Convert.ToString(jsonResponse.Result)); }

            return new List<ProductDTO>();
        }
    }
}
