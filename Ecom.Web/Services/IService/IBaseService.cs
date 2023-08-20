using ECOM.Services.CouponAPI.Models.DTO;
using ECOM.Web.Models;

namespace ECOM.Web.Services.IService
{
    public interface IBaseService
    {
        Task<ResponseDTO?>  SendAsync(RequestDTO requestDTO);
    }
}
