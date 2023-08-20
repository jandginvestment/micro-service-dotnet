using Microsoft.AspNetCore.Mvc;

namespace ECOM.Web.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}