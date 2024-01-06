using System.ComponentModel.DataAnnotations;

namespace ECOM.Services.OrderAPI.Models;

public class OrderHeader
{
    [Key]
    public required int OrderHeaderID { get; set; }
    public required string UserID { get; set; }
    public string? CouponCode { get; set; }
    public double Discount { get; set; } = 0;
    public double OrderTotal { get; set; } = 0;
    public string? Name { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public string? Email { get; set; }
    public string? Phone { get; set; }

    public DateTime? OrderOn { get; set; }
    public string? Status { get; set; }

    public string? PaymentIndentID { get; set; }
    public string? StripeSessionID { get; set; }
    public IEnumerable<OrderDetail> OrderDetails { get; set; }

}
