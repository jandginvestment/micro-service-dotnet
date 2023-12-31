﻿using ECOM.Web.Models;
using ECOM.Web.Services.IService;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using static ECOM.Web.Utility.StaticDetails;

namespace ECOM.Web.Services;

public class BaseService : IBaseService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ITokenProvider _tokenProvider;

    public BaseService(IHttpClientFactory httpClientFactory, ITokenProvider tokenProvider)
    {
        _httpClientFactory = httpClientFactory;
        _tokenProvider = tokenProvider;
    }
    public async Task<ResponseDTO?> SendAsync(RequestDTO requestDTO, bool withBearer = true)
    {

        try
        {
            HttpClient client = _httpClientFactory.CreateClient("EComAPI");

            HttpRequestMessage message = new();

            if (requestDTO.ContentType == ContentType.MultipartFormData)
            {
                var content = new MultipartFormDataContent();

                foreach (var prop in requestDTO.Data.GetType().GetProperties())
                {
                    var value = prop.GetValue(requestDTO.Data);
                    if (value is FormFile)
                    {
                        var file = (FormFile)value;
                        if (file != null)
                        {
                            content.Add(new StreamContent(file.OpenReadStream()), prop.Name, file.FileName);
                        }
                    }
                    else
                    {
                        content.Add(new StringContent(value != null ? value.ToString() : string.Empty), prop.Name);
                    }
                }
                message.Content = content;
            }
            else
            {
                if (requestDTO.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(requestDTO.Data), Encoding.UTF8, "application/json");
                }
            }


            //message.Headers.Add("Content-Type", "application/json");

            //place for Token
            if (withBearer)
            {
                var token = _tokenProvider.GetToken();
                message.Headers.Add("Authorization", $"Bearer {token}");
            }

            message.RequestUri = new Uri(requestDTO.Url);
            if (requestDTO.Data != null) { message.Content = new StringContent(JsonConvert.SerializeObject(requestDTO.Data), encoding: Encoding.UTF8, mediaType: "application/json"); }

            switch (requestDTO.APIType)
            {
                case APIType.GET:
                    message.Method = HttpMethod.Get;
                    break;
                case APIType.POST:
                    message.Method = HttpMethod.Post;
                    break;
                case APIType.PUT:
                    message.Method = HttpMethod.Put;
                    break;
                case APIType.DELETE:
                    message.Method = HttpMethod.Delete;
                    break;
                default:
                    message.Method = HttpMethod.Get;
                    break;
            }

            HttpResponseMessage? response = await client.SendAsync(message);

            switch (response.StatusCode)
            {
                case HttpStatusCode.NotFound:
                    return new() { IsSuccess = false, Message = "Not Found" };
                case HttpStatusCode.Forbidden:
                    return new() { IsSuccess = false, Message = "Forbidden" };
                case HttpStatusCode.Unauthorized:
                    return new() { IsSuccess = false, Message = "Unauthorized" };
                case HttpStatusCode.InternalServerError:
                    return new() { IsSuccess = false, Message = "Internal Server Error" };
                default: return JsonConvert.DeserializeObject<ResponseDTO>(await response.Content.ReadAsStringAsync());
            }
        }
        catch (Exception ex)
        {

            return new ResponseDTO
            {

                IsSuccess = false,
                Message = ex.Message,
            };
        }
    }
}
