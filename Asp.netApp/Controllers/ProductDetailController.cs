using Asp.netApp.Areas.Admin.Data;
using Asp.netApp.Areas.Admin.Models.ViewModels.Product;
using Asp.netApp.Areas.Admin.Services.Auth;
using Asp.netApp.Areas.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Asp.netApp.Controllers
{
    public class ProductDetailController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly LanguageService _languageService;
        private readonly string _currentLanguage;
        private LanguageService _localization;
        public ProductDetailController(ApplicationDbContext db, LanguageService languageService, AuthService authService, IMapper mapper, LanguageService _lang)
        {
            this._context = db;
            this._mapper = mapper;
            _localization = _lang;
            _currentLanguage = languageService.GetCurrentLanguage();
        }
        [HttpGet]
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product1 = await(from product in _context.Products
                                 join productLang in _context.ProductLanguages
                                 on product.Id equals productLang.ProductId
                                 where productLang.Language.Canonical == _currentLanguage && productLang.ProductId == id
                                 select new ProductEditViewModel
                                 {
                                     Id = product.Id,
                                     Price = product.Price,
                                     SalePrice = product.SalePrice,
                                     Publish = product.Publish != null ? "1" : "0",
                                     Image = product.Image,
                                     Category = product.Category.CategoryLanguages.ElementAt(0).CategoryName,
                                     Brand = product.Brand,
                                     Album = product.Album,
                                     ProductName = productLang.ProductName,
                                     Description = productLang.Description

                                 }).FirstOrDefaultAsync();


            if (product1 == null)
            {
                return NotFound();
            }
            
           

            return View(product1);
        }
    }
}
