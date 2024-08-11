using Asp.netApp.Areas.Admin.Constants;
using Asp.netApp.Areas.Admin.Data;
using Asp.netApp.Areas.Admin.Models.DataModel;
using Asp.netApp.Areas.Admin.Models.ViewModel.Auth;
using Asp.netApp.Areas.Admin.Services.Auth;
using Asp.netApp.Areas.Admin.Services.Role;
using Authorize_authentication.Areas.Admin.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;


namespace Asp.netApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly AuthService _authService;

        private readonly RoleService _roleService;
        public AuthController(ApplicationDbContext db, AuthService authService, RoleService roleService)
        {
            this._context = db;
            this._authService = authService;
            this._roleService = roleService;
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel request)
        {
            List<string> roles = new List<string>() { UserRole.ADMIN , UserRole.STAFF, UserRole.USER, UserRole.MANAGER};
            //validate
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            var hashPassword = Utility1.MD5Hash(request.Password);

            //check username password
            var acc = _context.Accounts.FirstOrDefault(x => x.Username == request.Username && x.Password == hashPassword);
            if (acc != null)
            {
                var id = acc.AccountId;
                string roleName = "";
                var data = _roleService.GetRoleByIdAsync(acc.RoleId);
                if ( data!= null)
                {
                    roleName = data.Result.RoleName;
                }
               

                var identity = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.Name, acc.Username),
                    new Claim(ClaimTypes.GivenName, acc.Fullname),
                    new Claim(ClaimTypes.NameIdentifier, acc.AccountId),
                    new Claim(ClaimTypes.Role, roleName) // Only one role claim
                ], "login");

              
                var principle = new ClaimsPrincipal(identity);

                var login = HttpContext.SignInAsync(principle);

                //check role admin to come in
                await _authService.CheckRole(id, roles);
     
                //lưu vào cookie

                //đúng thì vào sai thì out
                return RedirectToRoute(new { controller = "Dashboard", action = "Index" });
            }
            else
            {
                ViewBag.error = "Đăng nhập sai";
                return View(request);
            }

        }

        public async Task<IActionResult> Logout()
        {
            // Clear the existing external cookie
            await HttpContext.SignOutAsync();

            return RedirectToRoute(new { controller = "Auth", action = "Login" });
        }

      
    }
}
