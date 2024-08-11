using Asp.netApp.Areas.Admin.Constants;
using Asp.netApp.Areas.Admin.Models.ViewModels.Account;
using Asp.netApp.Areas.Admin.Resources;
using Asp.netApp.Areas.Admin.Services.Auth;
using Asp.netApp.Areas.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Asp.netApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly AuthService authService;

        private LanguageService _localization;

        public DashboardController(AuthService authService, LanguageService _lang) { 
        
            this.authService = authService;
            _localization = _lang;

        }
        public async Task<IActionResult> Index()
        {
            List<string> roles = new List<string>() { UserRole.ADMIN, UserRole.STAFF, UserRole.MANAGER};
            await authService.CheckRole(roles);


            return View();
        }

        public IActionResult ChangeLanguage(string culture)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)), new CookieOptions()
            {
                Expires = DateTimeOffset.UtcNow.AddYears(1)
            });
           
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
