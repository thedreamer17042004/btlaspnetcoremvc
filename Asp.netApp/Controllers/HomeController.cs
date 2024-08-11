using Asp.netApp.Areas.Admin.Data;
using Asp.netApp.Areas.Admin.Models.DataModels;
using Asp.netApp.Areas.Admin.Services.Auth;
using Asp.netApp.Areas.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Drawing.Printing;
using Microsoft.EntityFrameworkCore;

namespace Asp.netApp.Controllers
{
    public class HomeController : Controller
    {
      
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly LanguageService _languageService;
        private readonly string _currentLanguage;
        private LanguageService _localization;
        public HomeController(ApplicationDbContext db, LanguageService languageService, AuthService authService, IMapper mapper, LanguageService _lang)
        {
            this._context = db;
            this._mapper = mapper;
            _localization = _lang;
            _currentLanguage = languageService.GetCurrentLanguage();
        }
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
             .Select(c => new
             {
                 Product = c,
                 ProductLanguage = c.ProductLanguages
                     .FirstOrDefault(cl => cl.Language.Canonical.Equals(_currentLanguage))
             }).ToListAsync();


            var processedProducts = products
                .Where(c => c.ProductLanguage != null)
                .Select(c =>
                {
                    c.Product.ProductLanguages = new List<ProductLanguage> { c.ProductLanguage };
                    return c.Product;
                })
                .Take(10).ToList();

            ViewBag.products = processedProducts;

            return View();
        }
    }
}
