using System.ComponentModel.DataAnnotations;

namespace ECOM.Services.CouponAPI.Models
{
    public class Coupon
    {
        [Key]
        public int CouponId { get; set; }
        public required string CouponCode { get; set; }
        public double DiscountAmount { get; set; }
        public double MinimumAmount { get; set; }
    }
}
