using System.ComponentModel.DataAnnotations;

namespace ECOM.Services.ProductAPI.Models.DTO
{
    public class ProductDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ProductId { get; set; }
        public string CategoryName { get; set; }
        public string ImageUrl { get; set; }
        public double Price { get; set; }
    }
}
