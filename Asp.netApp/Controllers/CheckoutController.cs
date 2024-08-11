using Microsoft.AspNetCore.Mvc;

namespace Asp.netApp.Controllers
{
    public class CheckoutController : Controller
    {
        [HttpGet("checkout")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
