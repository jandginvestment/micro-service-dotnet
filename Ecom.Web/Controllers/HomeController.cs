using ECOM.Web.Models;
using ECOM.Web.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;

namespace ECOM.Web.Controllers;

public class HomeController : Controller
{
    private readonly IProductService _productService;
    private readonly IShoppingCartService _shoppingCartService;
    public HomeController(IProductService productService, IShoppingCartService shoppingCartService)
    {
        _productService = productService;
        _shoppingCartService = shoppingCartService;
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

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Details(ProductDTO product)
    {
        ShoppingCartDTO shoppingCart = new ShoppingCartDTO()
        {

            CartHeader = new CartHeaderDTO()
            {
                UserID = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)?.Value,
                CartHeaderID = 0
            },
            CartDetails = new List<CartDetailDTO>() { new CartDetailDTO() { Count = product.Count, ProductID = product.ProductId } }
        };
        var response = await _shoppingCartService.CartUpsertAsync(shoppingCart);
        if (response != null && response.IsSuccess)
        {
            TempData["success"] = "Item has been added to the shopping cart";
            return RedirectToAction(nameof(Index));
        }
        else { TempData["error"] = response?.Message; }
        return View(product);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current.Id ?? HttpContext.TraceIdentifier });
    }
}