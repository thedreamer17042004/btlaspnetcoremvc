using Microsoft.AspNetCore.Mvc;

namespace Asp.netApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Forbidden()
        {
            return View("~/Areas/Admin/Views/Error/Forbidden.cshtml");

        }

        public IActionResult NotFound()
        {
            return View("~/Areas/Admin/Views/Error/NotFound.cshtml");

        }
    }
}
