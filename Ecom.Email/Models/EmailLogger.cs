namespace Ecom.EmailAPI.Models;

public class EmailLogger
{
    public int ID { get; set; }
    public string Email { get; set; }
    public DateTime? EmailSentOn { get; set; }
    public string? Message { get; set; }
}
