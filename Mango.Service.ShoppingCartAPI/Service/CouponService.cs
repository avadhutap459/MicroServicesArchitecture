using Mango.Service.ShoppingCartAPI.Model.Dto;
using Mango.Service.ShoppingCartAPI.Service.IService;
using Newtonsoft.Json;

namespace Mango.Service.ShoppingCartAPI.Service
{
    public class CouponService : ICouponService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public CouponService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<CouponDto> GetCoupon(string couponCode)
        {
            var client = _httpClientFactory.CreateClient("Coupon");
            var response = await client.GetAsync($"api/CouponAPI/GetByCouponCode/{couponCode}");
            var apiContent = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
            if (resp.IsSucess)
            {
                return JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(resp.Result));

            }

            return new List<CouponDto>();
        }
    }
}
