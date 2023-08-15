using ECOM.Services.CouponAPI.Models.DTO;
using ECOM.Web.Models;
using ECOM.Web.Services.IService;
using ECOM.Web.Utility;
using Microsoft.Extensions.DependencyInjection;
using static ECOM.Web.Utility.StaticDetails;

namespace ECOM.Web.Services;

public class CouponService : ICouponService
{
    private readonly IBaseService _bseService;

    public CouponService(IBaseService bseService)
    {
        _bseService = bseService;
    }
    public async Task<ResponseDTO?> CreateCouponAsync(CouponDTO coupon)
    {
        return await _bseService.SendAsync(new RequestDTO()
        {
            APIType = APIType.POST,
            Url = StaticDetails.CouponAPIBase + "/Post",
            Data = coupon

        });
    }

    public async Task<ResponseDTO?> DeleteCouponAsync(int couponId)
    {
        return await _bseService.SendAsync(new RequestDTO()
        {
            APIType = APIType.DELETE,
            Url = StaticDetails.CouponAPIBase + "/Delete" + "/"+couponId,

        });
    }

    public async Task<ResponseDTO?> GetAllCouponsAsync()
    {
         
        return await _bseService.SendAsync(new RequestDTO()
        {
            APIType =APIType.GET,
            Url =StaticDetails.CouponAPIBase+ "/Get",

        });
    }

    public async Task<ResponseDTO?> GetCouponAsync(string couponCode)
    {
        return await _bseService.SendAsync(new RequestDTO()
        {
            APIType = APIType.GET,
            Url = StaticDetails.CouponAPIBase + "/getCouponsByCode" +"/"+couponCode,

        });
    }

    public async Task<ResponseDTO?> GetCouponByIDAsync(int couponId)
    {

        return await _bseService.SendAsync(new RequestDTO()
        {
            APIType = APIType.GET,
            Url = StaticDetails.CouponAPIBase + "/Get" + "/" + couponId,

        });
    }

    public async Task<ResponseDTO?> UpdateCouponAsync(CouponDTO coupon)
    {
        return await _bseService.SendAsync(new RequestDTO()
        {
            APIType = APIType.PUT,
            Url = StaticDetails.CouponAPIBase + "/UpdateCoupon",
            Data = coupon

        });
    }
}
