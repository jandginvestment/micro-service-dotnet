using static ECOM.Web.Utility.StaticDetails;

namespace ECOM.Web.Models;
public class RequestDTO
{
    public APIType APIType { get; set; } = APIType.GET;
    public string RequestID { get; set; }
    public string Url { get; set; }
    public object Data { get; set; }
    public string AccessToken { get; set; }
    public ContentType ContentType { get; set; } = ContentType.Json;

}
