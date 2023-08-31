using ECOM.Services.ShoppingCartAPI.Models.DTO;

namespace ECOM.Services.ShoppingCartAPI.Services.IServices
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetProducts();
    }
}
