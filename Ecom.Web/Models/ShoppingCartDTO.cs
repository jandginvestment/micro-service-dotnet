namespace ECOM.Web.Models;


public class ShoppingCartDTO
{
    public CartHeaderDTO CartHeader { get; set; }
    public IEnumerable<CartDetailDTO>? CartDetails { get; set; } = Enumerable.Empty<CartDetailDTO>();

}
