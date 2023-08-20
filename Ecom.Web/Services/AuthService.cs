using System.Security.Claims;
using Ecom.Web.Services.IService;
using ECOM.Services.CouponAPI.Models.DTO;
using ECOM.Web.Models;
using ECOM.Web.Services.IService;
using ECOM.Web.Utility;
using static ECOM.Web.Utility.StaticDetails;

namespace Ecom.Web.Services;

public class AuthService : IAuthService
{
    private readonly IBaseService _bseService;

    public AuthService(IBaseService bseService)
    {
        _bseService = bseService;
    }
    public async Task<ResponseDTO?> AssignRoleAsync(RegistrationRequestDTO registrationRequest)
    {
        return await _bseService.SendAsync(new RequestDTO()
        {
            APIType = APIType.POST,
            Url = StaticDetails.AuthAPIBase + "/api/auth/assignRole",
            Data = registrationRequest

        });
    }

    public async Task<ResponseDTO?> LoginAsync(LoginRequestDTO loginRequest)
    {
        return await _bseService.SendAsync(new RequestDTO()
        {
            APIType = APIType.POST,
            Url = StaticDetails.AuthAPIBase + "/api/auth/login",
            Data = loginRequest

        });
    }

    public async Task<ResponseDTO?> RegisterAsync(RegistrationRequestDTO registrationRequest)
    {
        return await _bseService.SendAsync(new RequestDTO()
        {
            APIType = APIType.POST,
            Url = StaticDetails.AuthAPIBase + "/api/auth/register",
            Data = registrationRequest

        });
    }

   
}