using ECOM.Services.OrderAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ECOM.Services.OrderAPI.Data;

public class AppDBContext : DbContext
{
    public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
    {

    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrderHeader>().HasKey(o => o.OrderHeaderID);

        // Configure other entity mappings

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<OrderHeader> OrderHeaders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }

}

