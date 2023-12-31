﻿using ECOM.Web.Models;
using ECOM.Web.Services.IService;
using ECOM.Web.Utility;
using static ECOM.Web.Utility.StaticDetails;

namespace ECOM.Web.Services;

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

        }, withBearer: false);
    }

    public async Task<ResponseDTO?> LoginAsync(LoginRequestDTO loginRequest)
    {
        return await _bseService.SendAsync(new RequestDTO()
        {
            APIType = APIType.POST,
            Url = StaticDetails.AuthAPIBase + "/api/auth/login",
            Data = loginRequest

        }, withBearer: false);
    }

    public async Task<ResponseDTO?> RegisterAsync(RegistrationRequestDTO registrationRequest)
    {
        return await _bseService.SendAsync(new RequestDTO()
        {
            APIType = APIType.POST,
            Url = StaticDetails.AuthAPIBase + "/api/auth/register",
            Data = registrationRequest

        }, withBearer: false);
    }


}