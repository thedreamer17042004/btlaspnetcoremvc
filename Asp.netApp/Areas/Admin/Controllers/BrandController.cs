using Asp.netApp.Areas.Admin.Constants;
using Asp.netApp.Areas.Admin.Data;
using Asp.netApp.Areas.Admin.Models.DataModel;
using Asp.netApp.Areas.Admin.Models.ViewModels.Account;
using Asp.netApp.Areas.Admin.Services.Auth;
using Asp.netApp.Areas.Services;
using Authorize_authentication.Areas.Admin.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using Microsoft.EntityFrameworkCore;
using Asp.netApp.Areas.Admin.Models.ViewModels.Brand;
using Asp.netApp.Areas.Admin.Common;
using Microsoft.AspNetCore.Authorization;
using Asp.netApp.Controllers;
using Microsoft.Extensions.Localization;
using System.Drawing.Drawing2D;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Asp.netApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class BrandController : Controller
    {
        
        private readonly AuthService authService;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<BrandController> _localizer;

        public BrandController(ApplicationDbContext db, AuthService authService, IMapper mapper, IStringLocalizer<BrandController> localizer)
        {
            this._context = db;
            this.authService = authService;
            this._mapper = mapper;
            this._localizer = localizer;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string? status, string s, int page = 1)
        {
            List<string> roles = new List<string>() { UserRole.ADMIN };
            await authService.CheckRole(roles);

            int pagesize = 10;

            s = string.IsNullOrEmpty(s) ? "" : s;
            bool statusCheck = !string.IsNullOrEmpty(status) && status != "";

            var query = _context.Brands.AsQueryable();

            if (statusCheck)
            {
                query = query.Where(ac => ac.Active == (status.Equals("1") ? true : false));
            }
 

            query = query.Where(ac => ac.BrandName.ToLower().Contains(s.Trim()));

            var brands = await query.ToPagedListAsync(page, pagesize);


            ViewBag.brands = brands;
            ViewBag.s = s;
            ViewBag.status = status != null ? status : "";

            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            List<string> roles = new List<string>() { UserRole.ADMIN };
            await authService.CheckRole(roles);

            if (id == null)
            {
                return NotFound();
            }

            var cate = await _context.Brands
                .FirstOrDefaultAsync(m => id == m.BrandId);
            var brand1 = _mapper.Map<BrandViewModel>(cate);
            brand1.Active = cate.Active?"1":"0";
            if (cate == null)
            {
                return NotFound();
            }

            return View(brand1);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            List<string> roles = new List<string>() { UserRole.ADMIN };
            await authService.CheckRole(roles);

            return View(new BrandViewModel());
        }


        [HttpPost]
        public async Task<IActionResult> Create(BrandViewModel brand)
        {
            List<string> roles = new List<string>() { UserRole.ADMIN };
            await authService.CheckRole(roles);

            if (ModelState.IsValid)
            {
                int checkExist = await _context.Brands.CountAsync(br=>br.BrandName.Equals(brand.BrandName));
                if (checkExist>0)
                {
                    ViewBag.error = _localizer["errorExist"];
                    return View(brand);
                }

                //map acc to entity
                var brand1 = _mapper.Map<Brand>(brand);
                brand1.Active = brand.Active.Equals("1") ? true : false;
                brand1.Slug = SlugHelper.GenerateSlug(brand.BrandName);

                _context.Brands.Add(brand1);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(brand);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            List<string> roles = new List<string>() { UserRole.ADMIN };
            await authService.CheckRole(roles);

            if (id == null)
            {
                return NotFound();
            }

            var brand = await _context.Brands.FindAsync(id);
            var toView = _mapper.Map<BrandViewModel>(brand);
            toView.Active = brand.Active ? "1" : "0";
            ViewBag.Id = brand.BrandId;
            if (brand == null)
            {
                return NotFound();
            }

            return View(toView);
        }

        [HttpPost]
        public async Task<IActionResult> Edit( int id, BrandViewModel brand)
        {
            List<string> roles = new List<string>() { UserRole.ADMIN };
            await authService.CheckRole(roles);

            var checkExist = await _context.Brands.FirstOrDefaultAsync(a => a.BrandId == id);

            if (checkExist != null)
            {
                if (id != checkExist.BrandId)
                {
                    return NotFound();
                }
            }


            if (ModelState.IsValid)
            {
                try
                {
                    //map brand to entity
                    checkExist.BrandName = brand.BrandName;
                    checkExist.Slug = SlugHelper.GenerateSlug(brand.BrandName);
                    checkExist.Active = brand.Active.Equals("1") ? true : false;

                  
                    _context.Brands.Update(checkExist);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;

                }
                return RedirectToAction(nameof(Index));
            }

          
            return View(brand);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            List<string> roles = new List<string>() { UserRole.ADMIN };
            await authService.CheckRole(roles);

            if (id == null)
            {
                return NotFound();
            }

            var brand = await _context.Brands.FindAsync(id);
            var toView = _mapper.Map<BrandViewModel>(brand);
            ViewBag.Id = brand.BrandId;
            toView.Active = brand.Active ? "1" : "0";
            if (brand == null)
            {
                return NotFound();
            }

            return View(toView);
        }


        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            List<string> roles = new List<string>() { UserRole.ADMIN };
            await authService.CheckRole(roles);

            var br = await _context.Brands.FindAsync(id);
            if (br != null)
            {
                _context.Brands.Remove(br);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        
    }
}
