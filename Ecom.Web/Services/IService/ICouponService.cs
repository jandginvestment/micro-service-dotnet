using ECOM.Services.CouponAPI.Models.DTO;
using ECOM.Web.Models;

namespace ECOM.Web.Services.IService
{
    public interface ICouponService
    {
        Task<ResponseDTO?> GetCouponAsync(string couponCode);
        Task<ResponseDTO?> GetCouponByIDAsync(int couponId);
        Task<ResponseDTO?> UpdateCouponAsync(CouponDTO coupon);
        Task<ResponseDTO?> DeleteCouponAsync(int couponId);
        Task<ResponseDTO?> CreateCouponAsync(CouponDTO coupon);
        Task<ResponseDTO?> GetAllCouponsAsync();

    }
}
