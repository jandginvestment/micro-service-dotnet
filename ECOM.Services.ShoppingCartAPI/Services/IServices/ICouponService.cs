using ECOM.Services.ShoppingCartAPI.Models.DTO;

namespace ECOM.Services.ShoppingCartAPI.Services.IServices;

public interface ICouponService
{
    Task<CouponDTO> getCouponByCode(string couponCode);
}
