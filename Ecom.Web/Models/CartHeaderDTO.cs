using System.ComponentModel.DataAnnotations;

namespace ECOM.Web.Models;


public class CartHeaderDTO
{
    public required int CartHeaderID { get; set; }
    public required string UserID { get; set; }
    public string? CouponCode { get; set; }
    public double Discount { get; set; } = 0;
    public double CartTotal { get; set; } = 0;
    [Required]
    public string? Name { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    [Required]
    public string? Email { get; set; }
    [Required]
    public string? Phone { get; set; }
}
