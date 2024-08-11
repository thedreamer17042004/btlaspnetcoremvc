using Asp.netApp.Areas.Admin.Data;
using Asp.netApp.Areas.Admin.Models.DataModel;
using Microsoft.EntityFrameworkCore;




namespace Asp.netApp.Areas.Admin.Services.Role
{
    public class RoleService
    {

        private readonly ApplicationDbContext _context;

        public RoleService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<RoleC> GetRoleByIdAsync(int id)
        {
            return await _context.Roles.FindAsync(id);
        }

        public async Task<List<RoleC>> GetRolesAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<int?> GetRoleIdByNameAsync(string roleName)
        {
            var role = await _context.Roles
                .FirstOrDefaultAsync(r => r.RoleName == roleName);
            return role?.RoleId;
        }

        public async Task<List<RoleC>> GetRolesByName(List<string> roleName)
        {
            var role = await _context.Roles
                .Where(r => roleName.Contains(r.RoleName)).ToListAsync();
            return role;
        }



    }
}
