using System.ComponentModel.DataAnnotations;

namespace ECOM.Services.ProductAPI.Models;

public class Product
{
    public string Name { get; set; }
    public string Description { get; set; }
    [Required]
    public int ProductId { get; set; }
    public string CategoryName { get; set; }
    public string ImageUrl   { get; set; }
    [Range(0, 10000)]
    public double Price { get; set; }

}


