using ECOM.Web.Models;
using ECOM.Web.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ECOM.Web.Controllers;

public class HomeController : Controller
{
    private readonly IProductService _productService;
    public HomeController(IProductService productService)
    {
        _productService = productService;
    }
    public async Task<IActionResult> Index()
    {
        List<ProductDTO> productDTOList = new();

        ResponseDTO? response = await _productService.GetAllProductsAsync();

        if (response != null && response.IsSuccess)
        {
            productDTOList = JsonConvert.DeserializeObject<List<ProductDTO>>(Convert.ToString(response.Result));
        }
        else { TempData["error"] = response?.Message; }
        return View(productDTOList);
    }

    [Authorize]
    public async Task<IActionResult> Details(int productId)
    {

        var product = new ProductDTO();

        ResponseDTO? response = await _productService.GetProductByIDAsync(productId);

        if (response != null && response.IsSuccess)
        {
            product = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Result));
        }
        else { TempData["error"] = response?.Message; }
        return View(product);
    }
}