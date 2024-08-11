using Asp.netApp.Areas.Admin.Common;
using Asp.netApp.Areas.Admin.Constants;
using Asp.netApp.Areas.Admin.Data;
using Asp.netApp.Areas.Admin.Models.DataModel;
using Asp.netApp.Areas.Admin.Models.ViewModels.Category;
using Asp.netApp.Areas.Admin.Services.Auth;
using Asp.netApp.Areas.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace Asp.netApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly AuthService authService;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly LanguageService _languageService;
        private readonly string _currentLanguage;
        private readonly ILogger<CategoryController> _logger;
        public CategoryController(ApplicationDbContext db, AuthService authService, IMapper mapper, LanguageService languageService, ILogger<CategoryController> logger)
        {
            this._context = db;
            this.authService = authService;
            this._mapper = mapper;
            this._logger = logger;
            _currentLanguage = languageService.GetCurrentLanguage();
        }
        [HttpGet]
        public async Task<IActionResult> Index(string? status, string s, int page = 1)
        {
            List<string> roles = new List<string>() { UserRole.ADMIN };
            await authService.CheckRole(roles);

            int pagesize = 10;

            s = string.IsNullOrEmpty(s) ? "" : s;
            bool statusCheck = !string.IsNullOrEmpty(status) && status != "";


            var query = _context.Categories.Include(c => c.CategoryLanguages).ThenInclude(z => z.Language).AsQueryable();

            if (statusCheck)
            {
                query = query.Where(ac => ac.Active == (status.Equals("1") ? true : false));
            }

            query = query.Where(c => c.CategoryLanguages.Any(cl=> cl.CategoryName.Contains(s)));

            var categories = await query
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
                .ToPagedList(page, pagesize);

            ViewBag.categories = processedCategories;
            ViewBag.s = s;
            ViewBag.status = status != null ? status : "";
          
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            List<string> roles = new List<string>() { UserRole.ADMIN };
            await authService.CheckRole(roles);

            if (id == null)
            {
                return NotFound();
            }
            var category1 = await (from category in _context.Categories
                                    join categoryLang in _context.CategoryLanguages
                                    on category.Id equals categoryLang.CategoryId
                                    where categoryLang.Language.Canonical == _currentLanguage && categoryLang.CategoryId == id
                                    select new CategoryEditViewModel
                                    {
                                        Picture=category.Picture,
                                       Active = category.Active?"1":"0",
                                        CategoryName = categoryLang.CategoryName,
                                    }).FirstOrDefaultAsync();


            if (category1 == null)
            {
                return NotFound();
            }

            return View(category1);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            List<string> roles = new List<string>() { UserRole.ADMIN };
            await authService.CheckRole(roles);

            return View(new CategoryViewModel());
        }


        [HttpPost]
        public async Task<IActionResult> Create(CategoryViewModel ct, IFormFile? uploadFile)
        {
            List<string> roles = new List<string>() { UserRole.ADMIN };
            await authService.CheckRole(roles);


            if (ModelState.IsValid)
            {
                //var empDTO2 = mapper.Map<Employee, EmployeeDTO>(emp);
                var uniqueFileName = await FileUpload.UploadFile(uploadFile);
               
                Category ctn = new Category();

                ctn.Active = (ct.Status.Equals("1")?true:false);
   
                if (uniqueFileName!=null)
                {
                    ctn.Picture = uniqueFileName;
                }

               _context.Categories.Add(ctn);
                await _context.SaveChangesAsync();


                CategoryLanguage ctl = new CategoryLanguage();

                ctl.Language = _context.Languages.Where(l => l.Canonical == _currentLanguage).FirstOrDefault();
                ctl.CategoryId = ctn.Id;
                ctl.CategoryName = ct.Name;
                //generate slug 
                ctl.Slug = SlugHelper.GenerateSlug(ct.Name);

                _context.CategoryLanguages.Add(ctl);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
           
            return View(ct);
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

            var category1 = await (from category in _context.Categories
                                   join categoryLang in _context.CategoryLanguages
                                   on category.Id equals categoryLang.CategoryId
                                   where categoryLang.Language.Canonical == _currentLanguage && categoryLang.CategoryId == id
                                   select new CategoryEditViewModel
                                   {
                                       Picture = category.Picture,
                                       Active = category.Active?"1":"0",
                                      CategoryName = categoryLang.CategoryName,
        
                                   }).FirstOrDefaultAsync();


            var toView = _mapper.Map<CategoryEditViewModel>(category1);
            
            if (category1 == null)
            {
                return NotFound();
            }
            ViewBag.Id = id;
            ViewBag.oldPicture = toView.Picture;

            return View(toView);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(IFormFile? uploadFile, string? oldPicture, int id, CategoryEditViewModel cte)
        {
            List<string> roles = new List<string>() { UserRole.ADMIN };
            await authService.CheckRole(roles);

            var checkExist = await _context.Categories.FirstOrDefaultAsync(a => a.Id == id);

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
                        checkExist.Picture = uniqueFileName;
                    }
                    else
                    {
                        checkExist.Picture = oldPicture;
                    }

                    //map cte to entity
                    checkExist.Active = cte.Active.Equals("1")?true:false;

                    _context.Categories.Update(checkExist);
                    await _context.SaveChangesAsync();

                    var categoryLanguage = await _context.CategoryLanguages.FirstOrDefaultAsync(a => a.CategoryId == id && a.Language.Canonical == _currentLanguage);
                    if (categoryLanguage != null) {

                        categoryLanguage.CategoryName = cte.CategoryName;
                        categoryLanguage.Slug  = SlugHelper.GenerateSlug(cte.CategoryName);
                        _context.CategoryLanguages.Update(categoryLanguage);
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
            return View(cte);
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

            var category1 = await (from category in _context.Categories
                                   join categoryLang in _context.CategoryLanguages
                                   on category.Id equals categoryLang.CategoryId
                                   where categoryLang.Language.Canonical == _currentLanguage && categoryLang.CategoryId == id
                                   select new CategoryEditViewModel
                                   {
                                       Picture = category.Picture,
                                       Active = category.Active ? "1" : "0",
                                       CategoryName = categoryLang.CategoryName,

                                   }).FirstOrDefaultAsync();


            var toView = _mapper.Map<CategoryEditViewModel>(category1);

            if (category1 == null)
            {
                return NotFound();
            }
            ViewBag.Id = id;
            ViewBag.oldPicture = toView.Picture;

            return View(toView);
        }


        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            List<string> roles = new List<string>() { UserRole.ADMIN };
            await authService.CheckRole(roles);

            var ct = await _context.Categories.FindAsync(id);
            if (ct != null)
            {
                _context.Categories.Remove(ct);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
