using Microsoft.AspNetCore.Mvc;

namespace Asp.netApp.Controllers
{
    public class AboutController : Controller
    {
        [HttpGet("about-us")]
        public IActionResult Index()
        {
           
            return View();
        }
    }
}
