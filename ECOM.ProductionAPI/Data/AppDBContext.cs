using ECOM.Services.ProductAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ECOM.Services.ProductAPI.Data;

public class AppDBContext :DbContext
{
    public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Create a new ULID generator.
        
        modelBuilder.Entity<Product>().HasData(new Product
        {
            ProductId = 101,
            Name = "Samosa",
            Price = 15,
            Description = " Quisque vel lacus ac magna, vehicula sagittis ut non lacus.<br/> Vestibulum arcu turpis, maximus malesuada neque. Phasellus commodo cursus pretium.",
            ImageUrl = "https://placehold.co/603x403",
            CategoryName = "Appetizer"
        });
        modelBuilder.Entity<Product>().HasData(new Product
        {
            ProductId = 10,
            Name = "Paneer Tikka",
            Price = 13.99,
            Description = " Quisque vel lacus ac magna, vehicula sagittis ut non lacus.<br/> Vestibulum arcu turpis, maximus malesuada neque. Phasellus commodo cursus pretium.",
            ImageUrl = "https://placehold.co/602x402",
            CategoryName = "Appetizer"
        });
        modelBuilder.Entity<Product>().HasData(new Product
        {
            ProductId = 1,
            Name = "Sweet Pie",
            Price = 10.99,
            Description = " Quisque vel lacus ac magna, vehicula sagittis ut non lacus.<br/> Vestibulum arcu turpis, maximus malesuada neque. Phasellus commodo cursus pretium.",
            ImageUrl = "https://placehold.co/601x401",
            CategoryName = "Dessert"
        });
        modelBuilder.Entity<Product>().HasData(new Product
        {
            ProductId = 1001,
            Name = "Pav Bhaji",
            Price = 15,
            Description = " Quisque vel lacus ac magna, vehicula sagittis ut non lacus.<br/> Vestibulum arcu turpis, maximus malesuada neque. Phasellus commodo cursus pretium.",
            ImageUrl = "https://placehold.co/600x400",
            CategoryName = "Entree"
        });
    }

    public DbSet<Product> Products { get; set; }
}

