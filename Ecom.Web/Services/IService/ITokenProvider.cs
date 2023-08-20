namespace Ecom.Web.Services.IService
{
    public interface ITokenProvider
    {
        void SetToken(string token);
        void GetToken();
        void ClearToken();
    }
}
