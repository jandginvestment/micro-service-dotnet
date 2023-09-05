using ECOM.Web.Models;

namespace ECOM.Web.Services.IService;

public interface IShoppingCartService
{
    Task<ResponseDTO?> CartUpsertAsync(ShoppingCartDTO ShoppingCart);
    Task<ResponseDTO?> ApplyCouponAsync(ShoppingCartDTO shoppingCart);
    Task<ResponseDTO?> RemoveCouponAsync(ShoppingCartDTO ShoppingCart);
    Task<ResponseDTO?> DeleteShoppingCartAsync(int shoppingCartDetailID);
    Task<ResponseDTO?> GetShoppingCartAsync(string userID);
}
