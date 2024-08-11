using Microsoft.AspNetCore.Mvc;

namespace Asp.netApp.Controllers
{
    public class ShoppingCart : Controller
    {
        [HttpGet("cart")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
