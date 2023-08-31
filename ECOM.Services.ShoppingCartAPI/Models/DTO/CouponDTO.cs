namespace ECOM.Services.ShoppingCartAPI.Models.DTO;

public class CouponDTO
{
    public int CouponId { get; set; }
    public required string CouponCode { get; set; }
    public double DiscountAmount { get; set; }
    public double MinimumAmount { get; set; }
}
