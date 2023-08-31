namespace ECOM.Services.ShoppingCartAPI.Models.DTO;

public class ProductDTO
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int ProductID { get; set; }
    public string CategoryName { get; set; }
    public double Price { get; set; }
    public string? ImageUrl { get; set; }
    public string? ImageLocalPath { get; set; }
    public IFormFile? Image { get; set; }
}
