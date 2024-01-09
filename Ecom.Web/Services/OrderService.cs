using ECOM.Web.Models;
using ECOM.Web.Services.IService;
using ECOM.Web.Utility;
using static ECOM.Web.Utility.StaticDetails;

namespace ECOM.Web.Services;

public class OrderService : IOrderService
{
    private readonly IBaseService _baseService;

    public OrderService(IBaseService baseService)
    {
        _baseService = baseService;
    }

    public async Task<ResponseDTO?> CreateOrderAsync(ShoppingCartDTO shoppingCart)
    {
        return await _baseService.SendAsync(new RequestDTO()
        {
            APIType = APIType.POST,
            Url = StaticDetails.OrderAPIBase + "/CreateOrder",
            Data = shoppingCart
        });
    }


}
