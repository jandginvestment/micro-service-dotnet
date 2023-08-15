using ECOM.Services.AuthAPI.Models.DTO;

namespace ECOM.Services.AuthAPI.Services.IServices;

public interface IAuthService
{
    Task<string> Register(RegistrationRequestDTO registrationRequest);
    Task<LoginResponseDTO> Login(LoginRequestDTO loginRequest);
    Task<bool> AssignRole(string email, string roleName);
}
