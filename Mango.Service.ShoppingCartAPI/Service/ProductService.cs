using Mango.Service.ShoppingCartAPI.Model.Dto;
using Mango.Service.ShoppingCartAPI.Models.Dto;
using Mango.Service.ShoppingCartAPI.Service.IService;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Mango.Service.ShoppingCartAPI.Service
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ProductService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            var client = _httpClientFactory.CreateClient("Product");
            var response = await client.GetAsync($"api/ProductAPI");
            var apiContent = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
            if(resp.IsSucess)
            {
                return JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(Convert.ToString(resp.Result));

            }

            return new List<ProductDto>();
        }
    }
}
