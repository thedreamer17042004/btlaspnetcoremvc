using Microsoft.AspNetCore.Mvc;

namespace Asp.netApp.Controllers
{
    public class BlogDetailController : Controller
    {
        [HttpGet("blog-detail")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
