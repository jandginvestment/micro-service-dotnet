using ECOM.Web.Models;

namespace ECOM.Web.Services.IService;

public interface IOrderService
{
    Task<ResponseDTO?> CreateOrderAsync(ShoppingCartDTO ShoppingCart);

}
