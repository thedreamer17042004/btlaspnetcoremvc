using Asp.netApp.Areas.Admin.Constants;
using Asp.netApp.Areas.Admin.Data;
using Asp.netApp.Areas.Admin.Models.DataModel;
using Asp.netApp.Areas.Admin.Models.DataModels;
using Asp.netApp.Areas.Admin.Services.Auth;
using Asp.netApp.Areas.Services;
using Asp.netApp.Models.ViewModels.Category;
using Asp.netApp.Models.ViewModels.Product;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using X.PagedList;

namespace Asp.netApp.Controllers
{
    public class ShopController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly LanguageService _languageService;
        private readonly string _currentLanguage;
        private LanguageService _localization;
        public ShopController(ApplicationDbContext db, LanguageService languageService, AuthService authService, IMapper mapper, LanguageService _lang)
        {
            this._context = db;
            this._mapper = mapper;
            _localization = _lang;
            _currentLanguage = languageService.GetCurrentLanguage();
        }



        [HttpGet("shop")]
        public async Task<IActionResult> Index(string brandId, string categoryId, string? publish, string s, float? fromPrice, float? toPrice, int page = 1)
        {
          
            int pagesize = 10;

            s = string.IsNullOrEmpty(s) ? "" : s;
            int brandId1 = string.IsNullOrEmpty(brandId) ? -1 : int.Parse(brandId);
            int categoryId1 = string.IsNullOrEmpty(categoryId) ? -1 : int.Parse(categoryId);
            bool statusCheck = !string.IsNullOrEmpty(publish) && publish != "";
            fromPrice = fromPrice ?? 0;
            toPrice = toPrice ?? -1;


            var query = _context.Products.AsQueryable();

            if (brandId1 != -1)
            {
                query = query.Where(ac => ac.BrandId == brandId1);
            }

            if (categoryId1 != -1)
            {
                query = query.Where(ac => ac.CategoryId == categoryId1);
            }

            if (toPrice != -1)
            {
                query = query.Where(ac => ac.Price >= fromPrice && ac.Price <= toPrice);
            }
            else
            {
                query = query.Where(ac => ac.Price >= fromPrice);
            }

            if (statusCheck)
            {
                query = query.Where(ac => ac.Publish == (publish.Equals("1") ? true : false));
            }

            query = query.Where(c => c.ProductLanguages.Any(cl => cl.ProductName.Contains(s)));

            var products = await query
               .Select(c => new
               {
                   Product = c,
                   ProductLanguage = c.ProductLanguages
                       .FirstOrDefault(cl => cl.Language.Canonical.Equals(_currentLanguage))
               })
               .ToListAsync();


            var processedProducts = products
                .Where(c => c.ProductLanguage != null)
                .Select(c =>
                {
                    c.Product.ProductLanguages = new List<ProductLanguage> { c.ProductLanguage };
                    return c.Product;
                })
                .ToPagedList(page, pagesize);


            var processedCategories = await GetCategories();
            var brands = _context.Brands.Select(c=> new BrandView
            {
                BrandId =c.BrandId,
                BrandName = c.BrandName,
                Count = (c.Products!=null)? c.Products.Count():0

            }).ToList();
            var categories1 = processedCategories.Select(c => new CategoryView
            {
                Id = c.Id,
                CategoryName = c.CategoryLanguages.ElementAt(0).CategoryName,
                Count = (c.Products!=null) ? c.Products.Count(): 0

            }).ToList();
            ViewBag.categories = categories1;
            ViewBag.brands = brands;
            ViewBag.products = processedProducts;
            ViewBag.s = s;
            ViewBag.publish = publish != null ? publish : "";
            ViewBag.brandId = brandId != null ? brandId : "";
            ViewBag.categoryId = categoryId != null ? categoryId : "";
            ViewBag.fromPrice = fromPrice != null ? fromPrice : 0;
            ViewBag.toPrice = toPrice != null ? toPrice : -1;
            return View();
        }

        private async Task<List<Category>> GetCategories()
        {
            var categories = await _context.Categories
              .Select(c => new
              {
                  Category = c,
                  CategoryLanguage = c.CategoryLanguages
                      .FirstOrDefault(cl => cl.Language.Canonical.Equals(_currentLanguage))
              })
              .ToListAsync();

            // Ensure that each category only includes the specific CategoryLanguage
            var processedCategories = categories
                .Where(c => c.CategoryLanguage != null)
                .Select(c =>
                {
                    c.Category.CategoryLanguages = new List<CategoryLanguage> { c.CategoryLanguage };
                    return c.Category;
                })
                .ToList();

            return processedCategories;
        }

    }
}
