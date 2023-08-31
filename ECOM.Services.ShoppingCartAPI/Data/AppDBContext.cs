using ECOM.Services.ShoppingCartAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ECOM.Services.ShoppingCartAPI.Data;

public class AppDBContext : DbContext
{
    public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
    {

    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CartHeader>().HasKey(c => c.CartHeaderID);

        // Configure other entity mappings

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<CartHeader> CartHeaders { get; set; }
    public DbSet<CartDetail> CartDetails { get; set; }

}

