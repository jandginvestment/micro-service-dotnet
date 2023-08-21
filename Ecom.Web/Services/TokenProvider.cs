using Ecom.Web.Services.IService;
using ECOM.Web.Utility;
using Newtonsoft.Json.Linq;

namespace Ecom.Web.Services
{
    public class TokenProvider: ITokenProvider
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public TokenProvider(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public void SetToken(string token)
        {
           _contextAccessor.HttpContext?.Response.Cookies.Append(StaticDetails.Token,token);
        }

        public string? GetToken()
        {
            string? token = null;
            bool? hasToken = _contextAccessor.HttpContext?.Request.Cookies.TryGetValue(StaticDetails.Token, out token);

            return hasToken is not true ? string.Empty : token;

        }

        public void ClearToken()
        {
            _contextAccessor.HttpContext?.Response.Cookies.Delete(StaticDetails.Token);

        }
    }
}
