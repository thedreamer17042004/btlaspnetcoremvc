using Microsoft.AspNetCore.Mvc;

namespace Asp.netApp.Controllers
{
    public class ContactController : Controller
    {
        [HttpGet("contact")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
