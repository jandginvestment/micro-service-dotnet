using ECOM.Services.CouponAPI.Models.DTO;
using ECOM.Web.Models;
using ECOM.Web.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ECOM.Web.Controllers;

public class CouponController : Controller
{
    private readonly ICouponService _couponService;
    public CouponController(ICouponService couponService)
    {
        _couponService = couponService;
    }
    public async Task<IActionResult> CouponIndex()
    {
        List<CouponDTO> couponDTOList = new();

        ResponseDTO? response = await _couponService.GetAllCouponsAsync();

        if (response != null && response.IsSuccess)
        {
            couponDTOList = JsonConvert.DeserializeObject<List<CouponDTO>>(Convert.ToString(response.Result));
        }
        else { TempData["error"] = response?.Message; }
        return View(couponDTOList);
    }

    public async Task<IActionResult> CouponCreate()
    {

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CouponCreate(CouponDTO coupon)
    {

        if (ModelState.IsValid)
        {
            try
            {
                ResponseDTO? response = await _couponService.CreateCouponAsync(coupon);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(CouponIndex));
                }
                else { TempData["error"] = response?.Message; return View(coupon); }

            }
            catch (Exception e)
            {
                ModelState.AddModelError("General", e.Message);
                return RedirectToAction(nameof(Error));
            }
        }
        else
        {
            return View(coupon);
        }

    }


    public async Task<IActionResult> CouponDelete(int couponId)
    {
        try
        {
            ResponseDTO? response = await _couponService.GetCouponByIDAsync(couponId);

            if (response != null && response.IsSuccess)
            {
                CouponDTO? couponDTO = JsonConvert.DeserializeObject<CouponDTO>(Convert.ToString(response.Result));
                return View(couponDTO);
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
    public async Task<IActionResult> CouponDelete(CouponDTO coupon)
    {
        ResponseDTO? response = await _couponService.DeleteCouponAsync(coupon.CouponId);

        if (response != null && response.IsSuccess)
        {
            return RedirectToAction(nameof(CouponIndex));
        }
        else { TempData["error"] = response?.Message; return View(coupon); }

    }
}