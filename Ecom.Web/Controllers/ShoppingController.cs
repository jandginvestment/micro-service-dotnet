using ECOM.Web.Models;
using ECOM.Web.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace ECOM.Web.Controllers;

public class ShoppingCartController : Controller
{
    private readonly IShoppingCartService _shoppingCartService;
    public ShoppingCartController(IShoppingCartService shoppingCartService)
    {
        _shoppingCartService = shoppingCartService;
    }

    [Authorize]
    public async Task<IActionResult> CartIndex()
    {
        return View(await LoadShoppingCartOfLoggedInUser());
    }

    [Authorize]
    public async Task<IActionResult> RemoveCart(int shoppingCartDetailID)
    {
        var userID = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)?.Value;
        var response = await _shoppingCartService.DeleteShoppingCartAsync(shoppingCartDetailID);
        if (response != null & response.IsSuccess)
        {
            TempData["Success"] = "Cart Removed";
            return RedirectToAction(nameof(CartIndex));

        }

        return View();
    }

    [Authorize]
    public async Task<IActionResult> ApplyCoupon(ShoppingCartDTO shoppingCart)
    {
        var response = await _shoppingCartService.ApplyCouponAsync(shoppingCart);
        if (response != null & response.IsSuccess)
        {
            TempData["Success"] = "Coupon Applied";
            return RedirectToAction(nameof(CartIndex));

        }
        return View();
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> EmailCart(ShoppingCartDTO shoppingCart)
    {
        var cart = await LoadShoppingCartOfLoggedInUser();
        cart.CartHeader.Email = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email)?.Value;

        var response = await _shoppingCartService.EmailCartAsync(cart);
        if (response != null & response.IsSuccess)
        {
            TempData["Success"] = "Email Applied";
            return RedirectToAction(nameof(CartIndex));

        }
        return View();
    }

    [Authorize]
    public async Task<IActionResult> RemoveCoupon(ShoppingCartDTO shoppingCart)
    {
        var response = await _shoppingCartService.RemoveCouponAsync(shoppingCart);
        if (response != null & response.IsSuccess)
        {
            TempData["Success"] = "Coupon Removed";
            return RedirectToAction(nameof(CartIndex));

        }
        return View();
    }

    private async Task<ShoppingCartDTO> LoadShoppingCartOfLoggedInUser()
    {
        var userID = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)?.Value;
        var response = await _shoppingCartService.GetShoppingCartAsync(userID);
        if (response != null & response.IsSuccess)
        {
            return JsonConvert.DeserializeObject<ShoppingCartDTO>(Convert.ToString(response.Result));

        }
        return new ShoppingCartDTO();
    }
}