﻿using ECOM.Web.Models;

namespace ECOM.Web.Services.IService;

public interface IBaseService
{
    Task<ResponseDTO?> SendAsync(RequestDTO requestDTO, bool withBearer = true);
}
