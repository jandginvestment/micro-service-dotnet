using ECOM.Services.CouponAPI.Models.DTO;
using ECOM.Web.Models;
using ECOM.Web.Services.IService;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using static ECOM.Web.Utility.StaticDetails;

namespace ECOM.Web.Services
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BaseService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<ResponseDTO?> SendAsync(RequestDTO requestDTO)
        {

            try
            {
                HttpClient client = _httpClientFactory.CreateClient("EComAPI");

                HttpRequestMessage message = new();

                message.Headers.Add("Accept", "application/json");


                //message.Headers.Add("Content-Type", "application/json");

                //place for Token

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
}
