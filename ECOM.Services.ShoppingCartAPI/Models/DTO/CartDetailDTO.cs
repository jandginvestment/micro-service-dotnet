namespace ECOM.Services.ShoppingCartAPI.Models.DTO;

public class CartDetailDTO
{
    public int CartDetailID { get; set; }
    public int CartHeaderID { get; set; }
    public CartHeaderDTO? CartHeader { get; set; }
    public int ProductID { get; set; }
    public ProductDTO? Product { get; set; }
    public int Count { get; set; }
}
