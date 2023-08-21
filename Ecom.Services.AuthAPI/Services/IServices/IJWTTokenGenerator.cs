using ECOM.Services.AuthAPI.Models;

namespace ECOM.Services.AuthAPI.Services.IServices;

public interface IJWTTokenGenerator
{
    string GenerateToken(ApplicationUser applicationUser, IEnumerable<string> roles);
}