using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECOM.Services.ShoppingCartAPI.Models;

public class CartHeader
{
    [Key]
    public int CartHeaderID { get; set; }
    public required string UserID { get; set; }
    public string? CouponCode { get; set; }
    [NotMapped]
    public double Discount { get; set; } = 0;
    [NotMapped]
    public double CartTotal { get; set; } = 0;
}
