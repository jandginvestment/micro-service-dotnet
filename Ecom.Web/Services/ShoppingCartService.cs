using ECOM.Web.Models;
using ECOM.Web.Services.IService;
using ECOM.Web.Utility;
using static ECOM.Web.Utility.StaticDetails;

namespace ECOM.Web.Services;

public class ShoppingCartService : IShoppingCartService
{
    private readonly IBaseService _baseService;

    public ShoppingCartService(IBaseService baseService)
    {
        _baseService = baseService;
    }

    public async Task<ResponseDTO?> ApplyCouponAsync(ShoppingCartDTO shoppingCart)
    {
        return await _baseService.SendAsync(new RequestDTO()
        {
            APIType = APIType.POST,
            Url = StaticDetails.ShoppingCartAPIBase + "/ApplyCoupon",
            Data = shoppingCart
        });
    }

    public async Task<ResponseDTO?> CartUpsertAsync(ShoppingCartDTO shoppingCart)
    {
        return await _baseService.SendAsync(new RequestDTO()
        {
            APIType = APIType.POST,
            Url = StaticDetails.ShoppingCartAPIBase + "/Upsert",
            Data = shoppingCart
        });
    }

    public async Task<ResponseDTO?> DeleteShoppingCartAsync(int shoppingCartDetailID)
    {
        return await _baseService.SendAsync(new RequestDTO()
        {
            APIType = APIType.POST,
            Url = StaticDetails.ShoppingCartAPIBase + "/Remove",
            Data = shoppingCartDetailID
        });
    }

    public async Task<ResponseDTO?> EmailCartAsync(ShoppingCartDTO shoppingCart)
    {
        return await _baseService.SendAsync(new RequestDTO()
        {
            APIType = APIType.POST,
            Url = StaticDetails.ShoppingCartAPIBase + "/EmailCart",
            Data = shoppingCart
        });
    }

    public async Task<ResponseDTO?> GetShoppingCartAsync(string userID)
    {
        return await _baseService.SendAsync(new RequestDTO()
        {
            APIType = APIType.GET,
            Url = StaticDetails.ShoppingCartAPIBase + $"/Get/{userID}",
        });
    }

    public async Task<ResponseDTO?> RemoveCouponAsync(ShoppingCartDTO shoppingCart)
    {
        return await _baseService.SendAsync(new RequestDTO()
        {
            APIType = APIType.POST,
            Url = StaticDetails.ShoppingCartAPIBase + "/RemoveCoupon",
            Data = shoppingCart
        });
    }
}
