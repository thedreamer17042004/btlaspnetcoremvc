using Asp.netApp.Areas.Admin.Constants;
using Asp.netApp.Areas.Admin.Data;
using Asp.netApp.Areas.Admin.Exceptions;
using Asp.netApp.Areas.Admin.Models.DataModel;
using Asp.netApp.Areas.Admin.Services.Role;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace Asp.netApp.Areas.Admin.Services.Auth
{
    public class AuthService
    {
        private RoleService _roleService;
        private ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(RoleService roleService, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) { 
            this._roleService = roleService;
            this._context = context;
            _httpContextAccessor = httpContextAccessor;

        }

        public async Task<string> CheckRole(string userId, List<string> roleName)
        {
            List<int> roleids = new List<int>();
            //process to 
            var roleIds = await _roleService.GetRolesByName(roleName);

            foreach(var role in roleIds)
            {
                roleids.Add(role.RoleId);
            }
 
            var userRole = await _context.Accounts.FindAsync(userId);

            if (!roleids.Contains(userRole.RoleId)) {
                //return error
                return "forbidden";

            }

            return null;
            //return success
        }


        public async Task CheckRole(List<string> roles)
        {
            var userId = GetCurrentUserId();
            var roleCheckResult = await CheckRole(userId, roles);
            if (roleCheckResult != null) {
                throw new ForbiddenException("Forbidden");
            }
            await Task.CompletedTask; 
        }

        public string GetCurrentUserId()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return userId;
        }

    }
}
