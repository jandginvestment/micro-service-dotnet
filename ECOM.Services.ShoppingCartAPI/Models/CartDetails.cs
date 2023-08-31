using ECOM.Services.ShoppingCartAPI.Models.DTO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECOM.Services.ShoppingCartAPI.Models;

public class CartDetail
{
    [Key]
    public int CartDetailID { get; set; }
    public int CartHeaderID { get; set; }
    [ForeignKey(nameof(CartHeaderID))]
    public required CartHeader? CartHeader { get; set; }
    public int ProductID { get; set; }
    [NotMapped]
    public ProductDTO Product { get; set; }
    public int Count { get; set; }
}
