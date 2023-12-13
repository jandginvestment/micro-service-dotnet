namespace ECOM.Services.EmailAPI.Models.DTO;

public class ShoppingCartDTO
{
    public CartHeaderDTO CartHeader { get; set; }
    public IEnumerable<CartDetailDTO>? CartDetails { get; set; } = Enumerable.Empty<CartDetailDTO>();
}
