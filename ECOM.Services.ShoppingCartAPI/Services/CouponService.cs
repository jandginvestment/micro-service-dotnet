using ECOM.Services.ShoppingCartAPI.Models.DTO;
using ECOM.Services.ShoppingCartAPI.Services.IServices;
using Newtonsoft.Json;

namespace ECOM.Services.ShoppingCartAPI.Services;

public class CouponService : ICouponService
{
    private readonly IHttpClientFactory _httpClientFactory;
    public CouponService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    public async Task<CouponDTO> getCouponByCode(string couponCode)
    {

        var client = _httpClientFactory.CreateClient("Coupon");

        var response = await client.GetAsync($"/GetByCode/{couponCode}");
        var apicontent = await response.Content.ReadAsStringAsync();
        var jsonResponse = JsonConvert.DeserializeObject<ResponseDTO>(apicontent);

        if (jsonResponse != null && jsonResponse.IsSuccess) { return JsonConvert.DeserializeObject<CouponDTO>(Convert.ToString(jsonResponse.Result)); }

        return new CouponDTO();
    }
}
