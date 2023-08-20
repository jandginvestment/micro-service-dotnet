using ECOM.Services.AuthAPI.Data;
using ECOM.Services.AuthAPI.Models;
using ECOM.Services.AuthAPI.Models.DTO;
using ECOM.Services.AuthAPI.Services.IServices;
using Microsoft.AspNetCore.Identity;

namespace ECOM.Services.AuthAPI.Services;

public class AuthService : IAuthService
{
    private readonly AppDBContext _appDBContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IJWTTokenGenerator _jwtTokenGenerator;
    public AuthService(AppDBContext appDBContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, JWTTokenGenerator jWTTokenGenerator)
    {
        _appDBContext = appDBContext;
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtTokenGenerator = jWTTokenGenerator;

    }
    public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequest)
    {
        var user = _appDBContext.Users.FirstOrDefault(u => u.UserName.ToLower() == loginRequest.UserName.ToLower());
        if (user == null) { return new LoginResponseDTO() { User = null, Token = string.Empty }; }

        bool isValid = await _userManager.CheckPasswordAsync(user, loginRequest.Password);
        LoginResponseDTO response = new LoginResponseDTO();

        if (!isValid)
        {
            return new LoginResponseDTO() { User = null, Token = string.Empty };
        }
        else
        {
            var token = _jwtTokenGenerator.GenerateToken(user);
            response.User = new() { Email = user.Email, Name = user.Name, PhoneNumber = user.PhoneNumber };
            response.Token = token;
        }

        return response;
    }

    public async Task<bool> AssignRole(string email, string roleName)
    {
        var user = _appDBContext.Users.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
        if (user != null)
        {
            if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole() { Name = roleName, NormalizedName = roleName.ToUpper() }).GetAwaiter().GetResult();
            }
            await _userManager.AddToRoleAsync(user, roleName);
            return true;
        }

        return false;

    }

    public async Task<string> Register(RegistrationRequestDTO registrationRequest)
    {
        ApplicationUser user = new()
        {
            Email = registrationRequest.Email,
            UserName = registrationRequest.Email,
            NormalizedEmail = registrationRequest.Email.ToUpper(),
            Name = registrationRequest.Name,
            PhoneNumber = registrationRequest.PhoneNumber
        };

        try
        {
            var result = await _userManager.CreateAsync(user, registrationRequest.Password);

            if (result.Succeeded)
            {
                var returnedUser = _appDBContext.ApplicationUsers.First(u => u.UserName == user.Email);
                UserDTO userDTO = new() { Email = returnedUser.Email, Id = returnedUser.Id, Name = returnedUser.Email, PhoneNumber = returnedUser.PhoneNumber };
                return string.Empty;
            }
            else { var error = result.Errors.FirstOrDefault(); return $"{error.Code}:    {error.Description}"; }
        }
        catch (Exception e)
        {

            throw;
        }

        return "Error Occured, please tryagain later";
    }
}