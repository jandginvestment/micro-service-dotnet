using ECOM.Web.Models;
using ECOM.Web.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ECOM.Web.Controllers;

public class ProductController : Controller
{
    private readonly IProductService _productService;
    public ProductController(IProductService productService)
    {
        _productService = productService;
    }
    public async Task<IActionResult> ProductIndex()
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

    public async Task<IActionResult> ProductCreate()
    {

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ProductCreate(ProductDTO product)
    {

        if (ModelState.IsValid)
        {
            try
            {
                ResponseDTO? response = await _productService.CreateProductAsync(product);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
                else { TempData["error"] = response?.Message; return View(product); }

            }
            catch (Exception e)
            {
                ModelState.AddModelError("General", e.Message);
                return RedirectToAction(nameof(Error));
            }
        }
        else
        {
            return View(product);
        }

    }


    public async Task<IActionResult> ProductDelete(int productId)
    {
        try
        {
            ResponseDTO? response = await _productService.GetProductByIDAsync(productId);

            if (response != null && response.IsSuccess)
            {
                ProductDTO? productDTO = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Result));
                return View(productDTO);
            }
            else { TempData["error"] = response?.Message; }
        }
        catch (Exception)
        {

            throw;
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ProductDelete(ProductDTO product)
    {
        ResponseDTO? response = await _productService.DeleteProductAsync(product.ProductId);

        if (response != null && response.IsSuccess)
        {
            return RedirectToAction(nameof(ProductIndex));
        }
        else { TempData["error"] = response?.Message; return View(product); }

    }


    public async Task<IActionResult> ProductEdit(int productId)
    {
        ResponseDTO? response = await _productService.GetProductByIDAsync(productId);

        if (response != null && response.IsSuccess)
        {
            ProductDTO? product = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Result));
            return View(product);
        }
        else
        {
            TempData["error"] = response?.Message;
        }
        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> ProductEdit(ProductDTO productDto)
    {
        if (ModelState.IsValid)
        {
            ResponseDTO? response = await _productService.UpdateProductAsync(productDto);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Product updated successfully";
                return RedirectToAction(nameof(ProductIndex));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
        }
        return View(productDto);
    }
}