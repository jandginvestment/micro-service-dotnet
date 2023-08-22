using ECOM.Services.CouponAPI.Models;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore;

namespace ECOM.Services.CouponAPI.Data;

public class AppDBContext :DbContext
{
    public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Coupon>().HasData(new Coupon{
            CouponId = 1,
            CouponCode ="10P",
            DiscountAmount = 10.05,
            MinimumAmount = 200.5
        
        }) ;

        modelBuilder.Entity<Coupon>().HasData(new Coupon
        {
            CouponId = 2,
            CouponCode = "20P",
            DiscountAmount = 20.05,
            MinimumAmount = 400.5

        });
    }

    public DbSet<Coupon> Coupons { get; set; }
}

