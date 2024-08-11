using Microsoft.AspNetCore.Mvc;

namespace Asp.netApp.Controllers
{
    public class AuthController : Controller
    {
        [HttpGet("/login")]
        public IActionResult Index()
        {
            return View("Login");
        }

        [HttpGet("/register")]
        public IActionResult Register()
        {
            return View("Register");
        }
    }
}
