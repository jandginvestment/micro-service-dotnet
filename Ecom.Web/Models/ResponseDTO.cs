namespace ECOM.Web.Models;

public class ResponseDTO
{
    public object? Result { get; set; }
    public string Error { get; set; }
    public string Message { get; set; }
    public bool IsSuccess { get; set; }
}