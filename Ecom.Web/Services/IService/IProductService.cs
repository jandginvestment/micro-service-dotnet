using ECOM.Web.Models;

namespace ECOM.Web.Services.IService;

public interface IProductService
{
    Task<ResponseDTO?> GetProductAsync(string ProductCode);
    Task<ResponseDTO?> GetProductByIDAsync(int ProductId);
    Task<ResponseDTO?> UpdateProductAsync(ProductDTO Product);
    Task<ResponseDTO?> DeleteProductAsync(int ProductId);
    Task<ResponseDTO?> CreateProductAsync(ProductDTO Product);
    Task<ResponseDTO?> GetAllProductsAsync();
}
