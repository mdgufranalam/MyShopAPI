using Microsoft.AspNetCore.Mvc;

namespace MyShopAPI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
