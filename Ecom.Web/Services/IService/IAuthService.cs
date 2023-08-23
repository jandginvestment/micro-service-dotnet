using ECOM.Web.Models;

namespace ECOM.Web.Services.IService;

public interface IAuthService
{
    Task<ResponseDTO?> LoginAsync(LoginRequestDTO loginRequest);
    Task<ResponseDTO?> RegisterAsync(RegistrationRequestDTO registrationRequest);
    Task<ResponseDTO?> AssignRoleAsync(RegistrationRequestDTO registrationRequest);
}