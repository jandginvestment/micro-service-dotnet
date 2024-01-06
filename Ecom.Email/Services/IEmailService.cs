using ECOM.Services.EmailAPI.Models.DTO;

namespace Ecom.EmailAPI.Services;

public interface IEmailService
{
    Task EmailCartAndLog(ShoppingCartDTO shoppingCart);
}
