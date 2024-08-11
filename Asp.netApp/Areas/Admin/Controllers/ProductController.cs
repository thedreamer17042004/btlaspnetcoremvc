using Asp.netApp.Areas.Admin.Constants;
using Asp.netApp.Areas.Admin.Data;
using Asp.netApp.Areas.Admin.Models.DataModel;
using Asp.netApp.Areas.Admin.Models.DataModels;
using Asp.netApp.Areas.Admin.Models.ViewModels.Account;
using Asp.netApp.Areas.Admin.Services.Auth;
using Asp.netApp.Areas.Services;
using Authorize_authentication.Areas.Admin.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using Microsoft.EntityFrameworkCore;
using Asp.netApp.Areas.Admin.Models.ViewModels.Category;
using Asp.netApp.Areas.Admin.Models.ViewModels.Product;
using Asp.netApp.Areas.Admin.Common;

namespace Asp.netApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ProductController : Controller
    {
        private readonly AuthService authService;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly LanguageService _languageService;
        private readonly string _currentLanguage;
        private LanguageService _localization;

        public ProductController(ApplicationDbContext db, LanguageService languageService, AuthService authService, IMapper mapper, LanguageService _lang)
        {
            this._context = db;
            this.authService = authService;
            this._mapper = mapper;
            _localization = _lang;
            _currentLanguage = languageService.GetCurrentLanguage();
        }
        [HttpGet]
        public async Task<IActionResult> Index(string brandId, string categoryId, string? publish, string s, float? fromPrice , float? toPrice,int page = 1)
        {
            List<string> roles = new List<string>() { UserRole.ADMIN, UserRole.MANAGER };
            await authService.CheckRole(roles);

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

            if (toPrice!=-1)
            {
                query = query.Where(ac => ac.Price >= fromPrice && ac.Price <= toPrice);
            }else
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
            var brands = _context.Brands.ToList();
            ViewBag.categories = processedCategories;
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
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            List<string> roles = new List<string>() { UserRole.ADMIN, UserRole.MANAGER };
            await authService.CheckRole(roles);

            if (id == null)
            {
                return NotFound();
            }
            var product1 = await (from product in _context.Products
                                   join productLang in _context.ProductLanguages
                                   on product.Id equals productLang.ProductId
                                   where productLang.Language.Canonical == _currentLanguage && productLang.ProductId == id
                                   select new ProductEditViewModel
                                   {
                                       Price = product.Price,
                                       SalePrice = product.SalePrice,
                                       Publish = product.Publish!=null? "1" : "0",
                                       Image = product.Image,
                                       CategoryId = product.CategoryId,
                                       BrandId = product.BrandId,
                                       Album = product.Album,
                                       ProductName = productLang.ProductName,
                                       Description = productLang.Description
                                     
                                   }).FirstOrDefaultAsync();


            if (product1 == null)
            {
                return NotFound();
            }
            var processedCategories = await GetCategories();
            var brands = _context.Brands.ToList();
            ViewBag.categories = processedCategories;
            ViewBag.brands = brands;
            return View(product1);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            List<string> roles = new List<string>() { UserRole.ADMIN , UserRole.MANAGER };
            await authService.CheckRole(roles);

            var categories = await GetCategories();
            var brands = _context.Brands.ToList();
            var attributes = await GetAttributes();
            ViewBag.categories = categories;
            ViewBag.brands = brands;
            ViewBag.attributes = attributes;


            return View(new ProductEditViewModel());
        }


        [HttpPost]
        public async Task<IActionResult> Create(ProductEditViewModel prod, IFormFile? uploadFile)
        {
            List<string> roles = new List<string>() { UserRole.ADMIN, UserRole.MANAGER };
            await authService.CheckRole(roles);


            if (ModelState.IsValid)
            {
                //var empDTO2 = mapper.Map<Employee, EmployeeDTO>(emp);
                var uniqueFileName = await FileUpload.UploadFile(uploadFile);

                Product pro = new Product();

                pro.Publish = (prod.Publish.Equals("1") ? true : false);

                if (uniqueFileName != null)
                {
                    pro.Image = uniqueFileName;
                }
                pro.Price = prod.Price;
                pro.SalePrice = prod.SalePrice != null ? (float)prod.SalePrice : 0;
                pro.Brand = await _context.Brands.FindAsync(prod.BrandId);
                pro.Category = await _context.Categories.FindAsync(prod.CategoryId);


                _context.Products.Add(pro);
                await _context.SaveChangesAsync();


                ProductLanguage pl = new ProductLanguage();

                pl.Language = _context.Languages.Where(l => l.Canonical == _currentLanguage).FirstOrDefault();
                pl.ProductName = prod.ProductName;
                pl.Description = prod.Description;
                pl.Slug = SlugHelper.GenerateSlug(prod.ProductName);
                pl.Product = pro;


                _context.ProductLanguages.Add(pl);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
          
            var categories = await GetCategories();
            var brands = _context.Brands.ToList();
            ViewBag.categories = categories;
            ViewBag.brands = brands;
            return View(prod);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            List<string> roles = new List<string>() { UserRole.ADMIN, UserRole.MANAGER };
            await authService.CheckRole(roles);

            if (id == null)
            {
                return NotFound();
            }
            var product1 = await (from product in _context.Products
                                  join productLang in _context.ProductLanguages
                                  on product.Id equals productLang.ProductId
                                  where productLang.Language.Canonical == _currentLanguage && productLang.ProductId == id
                                  select new ProductEditViewModel
                                  {
                                      Price = product.Price,
                                      SalePrice = product.SalePrice,
                                      Publish = product.Publish != null ? "1" : "0",
                                      Image = product.Image,
                                      CategoryId = product.CategoryId,
                                      BrandId = product.BrandId,
                                      Album = product.Album,
                                      ProductName = productLang.ProductName,
                                      Description = productLang.Description

                                  }).FirstOrDefaultAsync();


            if (product1 == null)
            {
                return NotFound();
            }
            var processedCategories = await GetCategories();
            var brands = _context.Brands.ToList();
            ViewBag.categories = processedCategories;
            ViewBag.brands = brands;
            ViewBag.Id = id;
            return View(product1);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(IFormFile? uploadFile, string? oldPicture, int id, ProductEditViewModel prodEdit)
        {
            List<string> roles = new List<string>() { UserRole.ADMIN, UserRole.MANAGER };
            await authService.CheckRole(roles);

            var checkExist = await _context.Products.FirstOrDefaultAsync(a => a.Id == id);

            if (checkExist != null)
            {
                if (id != checkExist.Id)
                {
                    return NotFound();
                }
            }


            if (ModelState.IsValid)
            {
                try
                {
                    var uniqueFileName = await FileUpload.UploadFile(uploadFile);
                    if (uniqueFileName != null && uniqueFileName.Length > 0)
                    {
                        checkExist.Image = uniqueFileName;
                    }
                    else
                    {
                        checkExist.Image = oldPicture;
                    }

                    //map cte to entity
                    checkExist.Publish = prodEdit.Publish.Equals("1") ? true : false;
                    checkExist.Price = prodEdit.Price;
                    checkExist.SalePrice = (float)prodEdit.SalePrice;
                    checkExist.Brand = await _context.Brands.FindAsync(prodEdit.BrandId);
                    checkExist.Category = await _context.Categories.FindAsync(prodEdit.CategoryId);
                    _context.Products.Update(checkExist);
                    await _context.SaveChangesAsync();

                    var productLanguage = await _context.ProductLanguages.FirstOrDefaultAsync(a => a.ProductId == id && a.Language.Canonical == _currentLanguage);
                    if (productLanguage != null)
                    {

                        productLanguage.ProductName = prodEdit.ProductName;
                        productLanguage.Description = prodEdit.Description;
                        productLanguage.Slug = SlugHelper.GenerateSlug(prodEdit.ProductName);
                        _context.ProductLanguages.Update(productLanguage);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;

                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Id = id;
            ViewBag.oldPicture = oldPicture;
            var processedCategories = await GetCategories();
            var brands = _context.Brands.ToList();
            ViewBag.categories = processedCategories;
            ViewBag.brands = brands;
            return View(prodEdit);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            List<string> roles = new List<string>() { UserRole.ADMIN , UserRole.MANAGER };
            await authService.CheckRole(roles);

            if (id == null)
            {
                return NotFound();
            }
            var product1 = await (from product in _context.Products
                                  join productLang in _context.ProductLanguages
                                  on product.Id equals productLang.ProductId
                                  where productLang.Language.Canonical == _currentLanguage && productLang.ProductId == id
                                  select new ProductEditViewModel
                                  {
                                      Price = product.Price,
                                      SalePrice = product.SalePrice,
                                      Publish = product.Publish != null ? "1" : "0",
                                      Image = product.Image,
                                      CategoryId = product.CategoryId,
                                      BrandId = product.BrandId,
                                      Album = product.Album,
                                      ProductName = productLang.ProductName,
                                      Description = productLang.Description

                                  }).FirstOrDefaultAsync();


            if (product1 == null)
            {
                return NotFound();
            }
            var processedCategories = await GetCategories();
            var brands = _context.Brands.ToList();
            ViewBag.categories = processedCategories;
            ViewBag.brands = brands;
            ViewBag.Id = id;
            return View(product1);
        }


        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            List<string> roles = new List<string>() { UserRole.ADMIN , UserRole.MANAGER };
            await authService.CheckRole(roles);

            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
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
        private async Task<List<AttributeC>> GetAttributes()
        {
            var attributes = await _context.Attributes
              .Select(c => new
              {
                  Attribute = c,
                  AttributeLanguage = c.AttributeLanguages
                      .FirstOrDefault(cl => cl.Language.Canonical.Equals(_currentLanguage))
              })
              .ToListAsync();


            var processedAttributes = attributes
                .Where(c => c.AttributeLanguage != null)
                .Select(c =>
                {
                    c.Attribute.AttributeLanguages = new List<AttributeLanguage> { c.AttributeLanguage };
                    return c.Attribute;
                })
                .ToList();

            return processedAttributes;
        }

    }
}
