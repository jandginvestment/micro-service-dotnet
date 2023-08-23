using ECOM.Web.Models;
using ECOM.Web.Services.IService;
using ECOM.Web.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ECOM.Web.Controllers;

public class AuthController : Controller
{
    private readonly IAuthService _authService;
    private readonly ITokenProvider _tokenProvider;

    public AuthController(IAuthService authService, ITokenProvider tokenProvider)
    {

        _authService = authService;
        _tokenProvider = tokenProvider;
    }

    [HttpGet]
    public IActionResult Login()
    {
        LoginRequestDTO loginRequestDTO = new();
        return View(loginRequestDTO);

    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginRequestDTO loginRequest)
    {
        ResponseDTO loginResponse = await _authService.LoginAsync(loginRequest);

        try
        {
            if (loginResponse != null && loginResponse.IsSuccess)
            {
                LoginResponseDTO response =
                    JsonConvert.DeserializeObject<LoginResponseDTO>(Convert.ToString(loginResponse.Result));
                await SignInUserAsync(response);
                _tokenProvider.SetToken(response.Token);
                return RedirectToAction("Index", "Home");

            }
            else
            {
                ModelState.AddModelError("Error", loginResponse.Error + loginResponse.Message);
                return View(loginRequest);
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpGet]
    public IActionResult Register()
    {
        var roleList = new List<SelectListItem>(){
            new SelectListItem { Text = StaticDetails.RoleAdmin, Value = StaticDetails.RoleAdmin },
            new SelectListItem { Text = StaticDetails.RoleCustomer, Value = StaticDetails.RoleCustomer }};
        ViewBag.RoleList = roleList;

        return View();

    }



    [HttpPost]
    public async Task<IActionResult> RegisterAsync(RegistrationRequestDTO registrationRequest)
    {
        ResponseDTO registerResponse = await _authService.RegisterAsync(registrationRequest);
        ResponseDTO assignRoleResponse;

        try
        {
            if (registerResponse != null && registerResponse.IsSuccess)
            {
                if (string.IsNullOrEmpty(registrationRequest.Role))
                {
                    registrationRequest.Role = StaticDetails.RoleCustomer;
                }

                assignRoleResponse = await _authService.AssignRoleAsync(registrationRequest);

                if (assignRoleResponse != null && assignRoleResponse.IsSuccess)
                {
                    TempData["Success"] = StaticDetails.AssignRoleSuccess;

                    return RedirectToAction(nameof(Login));
                }
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        TempData["error"] = registerResponse?.Error;

        var roleList = new List<SelectListItem>()
            {
                new SelectListItem { Text = StaticDetails.RoleAdmin, Value = StaticDetails.RoleAdmin },
                new SelectListItem { Text = StaticDetails.RoleCustomer, Value = StaticDetails.RoleCustomer }
            };
        ViewBag.RoleList = roleList;

        return View(registrationRequest);


    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        _tokenProvider.ClearToken();
        return RedirectToAction("Index", "Home");

    }


    private async Task SignInUserAsync(LoginResponseDTO loginResponse)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(loginResponse.Token);
        if (jwt != null)
        {
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            var claims = new List<Claim> { new Claim(JwtRegisteredClaimNames.Email, jwt.Claims.FirstOrDefault(u=>u.Type == JwtRegisteredClaimNames.Email)?.Value),
                new Claim(JwtRegisteredClaimNames.Sub,  jwt.Claims.FirstOrDefault(u=>u.Type == JwtRegisteredClaimNames.Sub).Value),
                new Claim(ClaimTypes.Name,  jwt.Claims.FirstOrDefault(u=>u.Type == JwtRegisteredClaimNames.Email).Value),
                new Claim(ClaimTypes.Role,  jwt.Claims.FirstOrDefault(u=>u.Type == "role").Value),
                new Claim(JwtRegisteredClaimNames.Name,  jwt.Claims.FirstOrDefault(u=>u.Type == JwtRegisteredClaimNames.Name).Value) };

            identity.AddClaims(claims);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}