using Asp.netApp.Areas.Admin.Configs;
using Asp.netApp.Areas.Admin.Constants;
using Asp.netApp.Areas.Admin.Data;
using Asp.netApp.Areas.Admin.Models.DataModel;
using Asp.netApp.Areas.Admin.Models.ViewModels.Account;
using Asp.netApp.Areas.Admin.Services.Auth;
using Authorize_authentication.Areas.Admin.Models;
using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Data;
using X.PagedList;
using System.Security.Claims;
using Microsoft.Extensions.Localization;
using Asp.netApp.Areas.Admin.Resources;
using Asp.netApp.Areas.Services;

namespace Asp.netApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AccountController : Controller
    {
        private readonly AuthService authService;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private LanguageService _localization;

        public AccountController(ApplicationDbContext db, AuthService authService, IMapper mapper,LanguageService _lang)
        {
            this._context = db;
            this.authService = authService;
            this._mapper = mapper;
            _localization = _lang;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string? roleId,string? status, string s, int page = 1)
        {
            List<string> roles = new List<string>() { UserRole.ADMIN };
            await authService.CheckRole(roles);

            int pagesize = 10;

            s = string.IsNullOrEmpty(s) ? "" : s;
            bool roleIdc = !string.IsNullOrEmpty(roleId) && roleId != "";
            bool statusCheck = !string.IsNullOrEmpty(status) && status != "";

            var query = _context.Accounts.Where(ac => ac.Username.ToLower()!="admin").AsQueryable();

            if (statusCheck)
            {
                query = query.Where(ac => ac.Active == (status.Equals("1")?true:false));
            }


            if (roleIdc)
            {
                query = query.Where(ac => ac.RoleId == int.Parse(roleId));
            }

            query = query.Where(ac => ac.Username.ToLower().Contains(s.Trim()) || ac.Email.ToLower().Contains(s.Trim()));

            var accounts = await query.Include(a => a.Role).ToPagedListAsync(page, pagesize);
           
            var rolesList = _context.Roles.ToList();
            ViewBag.roles = rolesList;
            ViewBag.accounts = accounts;
            ViewBag.s = s;
            ViewBag.roleId = roleId!=null?roleId:"";
            ViewBag.status = status != null? status : "";
            
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Details(string? id)
        {
            List<string> roles = new List<string>() { UserRole.ADMIN };
            await authService.CheckRole(roles);

            if (id == null)
            {
                return NotFound();
            }

            var acc = await _context.Accounts.Include(r=> r.Role)
                .FirstOrDefaultAsync(m => id == m.AccountId);
            if (acc == null)
            {
                return NotFound();
            }

            return View(acc);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            List<string> roles = new List<string>() { UserRole.ADMIN };
            await authService.CheckRole(roles);

            var roles1 = _context.Roles.ToList();
            ViewBag.roles = roles1;

            return View(new AccountViewModel());
        }

      
        [HttpPost]
        public async Task<IActionResult> Create(AccountViewModel acc, IFormFile? uploadFile)
        {
            List<string> roles = new List<string>() { UserRole.ADMIN };
            await authService.CheckRole(roles);

            if (ModelState.IsValid)
            {

                //map acc to entity
                var hashPassword = Utility1.MD5Hash(acc.Password);
                var acc1 = _mapper.Map<Account>(acc);
                acc1.AccountId = Guid.NewGuid().ToString();
                acc1.Fullname = acc.Username;
                acc1.Password = hashPassword;

                //var empDTO2 = mapper.Map<Employee, EmployeeDTO>(emp);
                var uniqueFileName = "";
                if (uploadFile != null && uploadFile.Length > 0)
                {
                    uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(uploadFile.FileName);
                    var filePath = Path.Combine("wwwroot/images", uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await uploadFile.CopyToAsync(stream);
                    }

                }
                if (!uniqueFileName.Equals(""))
                {
                    acc1.Images =  uniqueFileName;
                }
               
               _context.Accounts.Add(acc1);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var roles1 = _context.Roles.ToList();
            ViewBag.roles = roles1;
            return View(acc);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            List<string> roles = new List<string>() { UserRole.ADMIN };
            await authService.CheckRole(roles);

            if (id == null)
            {
                return NotFound();
            }

            var acc = await _context.Accounts.FindAsync(id);
            var toView = _mapper.Map<AccountEditViewModel>(acc);
            toView.Password = "";
            if (acc == null)
            {
                return NotFound();
            }
         
            var roles1 = _context.Roles.ToList();
            ViewBag.roles = roles1;
    
            return View(toView);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(IFormFile? uploadFile, string? oldPicture, string id, AccountEditViewModel acc)
        {
            List<string> roles = new List<string>() { UserRole.ADMIN };
            await authService.CheckRole(roles);

            var checkExist = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountId == id);

            if (checkExist != null)
            {
                if (id != checkExist.AccountId)
                {
                   return NotFound();
                }
            }
          

            if (ModelState.IsValid)
            {
                try
                {
                    var uniqueFileName = "";
                    if (uploadFile!=null && uploadFile.Length > 0)
                    {
                        uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(uploadFile.FileName);
                        var filePath = Path.Combine("wwwroot/images", uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await uploadFile.CopyToAsync(stream);
                        }
                        checkExist.Images = uniqueFileName;
                    }
                    else
                    { 
                        checkExist.Images = oldPicture;
                    }

                    //map acc to entity
                    checkExist.Username = acc.Username;
                    checkExist.Email = acc.Email;
                    checkExist.FirstName = acc.FirstName;
                    checkExist.LastName = acc.LastName;
                    checkExist.Gender = acc.Gender==1?true:false;
                    checkExist.Active = acc.Active == 1 ? true : false;
                    checkExist.Phone = acc.Phone;
                    checkExist.Address = acc.Address;
                    checkExist.Birthday = acc.Birthday;
                    checkExist.RoleId = (int)acc.RoleId;
                    checkExist.Fullname = acc.Username;
                    checkExist.Password = checkExist.Password;

                   if (acc.Password!=null)
                    {
                        var hashPassword = Utility1.MD5Hash(acc.Password);
                        checkExist.Password = hashPassword;

                    }
                    //check account
                    _context.Accounts.Update(checkExist);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                    
                }
                return RedirectToAction(nameof(Index));
            }

            var roles1 = _context.Roles.ToList();
            ViewBag.roles = roles1;
            return View(acc);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            List<string> roles = new List<string>() { UserRole.ADMIN };
            await authService.CheckRole(roles);

            if (id == null)
            {
                return NotFound();
            }

            var acc = await _context.Accounts.Include(a=>a.Role)
                .FirstOrDefaultAsync(m => m.AccountId == id);
            if (acc == null)
            {
                return NotFound();
            }

            return View(acc);
        }

        
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            List<string> roles = new List<string>() { UserRole.ADMIN };
            await authService.CheckRole(roles);

            var acc = await _context.Accounts.FindAsync(id);
            if (acc != null)
            {
                _context.Accounts.Remove(acc);
            }
  
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
