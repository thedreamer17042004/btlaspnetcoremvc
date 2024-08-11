using Microsoft.AspNetCore.Mvc;

namespace Asp.netApp.Controllers
{
    public class BlogController : Controller
    {
        [HttpGet("blog")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
