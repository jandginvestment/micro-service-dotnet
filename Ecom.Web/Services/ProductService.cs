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
    public async Task<ResponseDTO?> CreateProductAsync(ProductDTO Product)
    {
        return await _bseService.SendAsync(new RequestDTO()
        {
            APIType = APIType.POST,
            Url = StaticDetails.ProductAPIBase + "/Post",
            Data = Product

        });
    }

    public async Task<ResponseDTO?> DeleteProductAsync(int ProductId)
    {
        return await _bseService.SendAsync(new RequestDTO()
        {
            APIType = APIType.DELETE,
            Url = StaticDetails.ProductAPIBase + "/Delete" + "/"+ProductId,

        });
    }

    public async Task<ResponseDTO?> GetAllProductsAsync()
    {
     
        return await _bseService.SendAsync(new RequestDTO()
        {
            APIType =APIType.GET,
            Url =StaticDetails.ProductAPIBase+ "/Get",

        });
    }

    public async Task<ResponseDTO?> GetProductAsync(string ProductCode)
    {
        return await _bseService.SendAsync(new RequestDTO()
        {
            APIType = APIType.GET,
            Url = StaticDetails.ProductAPIBase + "/getProductsByCode" +"/"+ProductCode,

        });
    }

    public async Task<ResponseDTO?> GetProductByIDAsync(int ProductId)
    {

        return await _bseService.SendAsync(new RequestDTO()
        {
            APIType = APIType.GET,
            Url = StaticDetails.ProductAPIBase + "/Get" + "/" + ProductId,

        });
    }

    public async Task<ResponseDTO?> UpdateProductAsync(ProductDTO Product)
    {
        return await _bseService.SendAsync(new RequestDTO()
        {
            APIType = APIType.PUT,
            Url = StaticDetails.ProductAPIBase + "/UpdateProduct",
            Data = Product

        });
    }
}
