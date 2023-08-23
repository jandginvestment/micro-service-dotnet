using ECOM.Web.Models;
using ECOM.Web.Services.IService;
using ECOM.Web.Utility;
using static ECOM.Web.Utility.StaticDetails;

namespace ECOM.Web.Services;

public class ProductService : IProductService
{
    private readonly IBaseService _bseService;

    public ProductService(IBaseService bseService)
    {
        _bseService = bseService;
    }
    public async Task<ResponseDTO?> CreateProductAsync(ProductDTO product)
    {
        return await _bseService.SendAsync(new RequestDTO()
        {
            APIType = APIType.POST,
            Url = StaticDetails.ProductAPIBase + "/Post",
            Data = product

        });
    }

    public async Task<ResponseDTO?> DeleteProductAsync(int productId)
    {
        return await _bseService.SendAsync(new RequestDTO()
        {
            APIType = APIType.DELETE,
            Url = StaticDetails.ProductAPIBase + "/Delete" + "/" + productId,

        });
    }

    public async Task<ResponseDTO?> GetAllProductsAsync()
    {

        return await _bseService.SendAsync(new RequestDTO()
        {
            APIType = APIType.GET,
            Url = StaticDetails.ProductAPIBase + "/Get",

        });
    }

    public async Task<ResponseDTO?> GetProductAsync(string productCode)
    {
        return await _bseService.SendAsync(new RequestDTO()
        {
            APIType = APIType.GET,
            Url = StaticDetails.ProductAPIBase + "/getProductsByCode" + "/" + productCode,

        });
    }

    public async Task<ResponseDTO?> GetProductByIDAsync(int productId)
    {

        return await _bseService.SendAsync(new RequestDTO()
        {
            APIType = APIType.GET,
            Url = StaticDetails.ProductAPIBase + "/Get" + "/" + productId,

        });
    }

    public async Task<ResponseDTO?> UpdateProductAsync(ProductDTO product)
    {
        return await _bseService.SendAsync(new RequestDTO()
        {
            APIType = APIType.PUT,
            Url = StaticDetails.ProductAPIBase + "/Put",
            Data = product,
            ContentType = StaticDetails.ContentType.MultipartFormData

        });
    }


}
