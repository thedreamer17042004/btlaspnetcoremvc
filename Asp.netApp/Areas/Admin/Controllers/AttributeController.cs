using Asp.netApp.Areas.Admin.Common;
using Asp.netApp.Areas.Admin.Constants;
using Asp.netApp.Areas.Admin.Data;
using Asp.netApp.Areas.Admin.Models.DataModel;
using Asp.netApp.Areas.Admin.Models.DataModels;
using Asp.netApp.Areas.Admin.Models.ViewModels.Category;
using Asp.netApp.Areas.Admin.Services.Auth;
using Asp.netApp.Areas.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using Microsoft.EntityFrameworkCore;
using Asp.netApp.Areas.Admin.Models.ViewModels.Attribute;

namespace Asp.netApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AttributeController : Controller
    {
        private readonly AuthService authService;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly LanguageService _languageService;
        private readonly string _currentLanguage;
        private readonly ILogger<AttributeController> _logger;
        public AttributeController(ApplicationDbContext db, AuthService authService, IMapper mapper, LanguageService languageService, ILogger<AttributeController> logger)
        {
            this._context = db;
            this.authService = authService;
            this._mapper = mapper;
            this._logger = logger;
            _currentLanguage = languageService.GetCurrentLanguage();
        }
        [HttpGet]
        public async Task<IActionResult> Index(string s, int page = 1)
        {
            List<string> roles = new List<string>() { UserRole.ADMIN, UserRole.STAFF };
            await authService.CheckRole(roles);

            int pagesize = 10;

            s = string.IsNullOrEmpty(s) ? "" : s;


            var query = _context.Attributes.Include(c => c.AttributeLanguages).ThenInclude(z => z.Language).AsQueryable();


            query = query.Where(c => c.AttributeLanguages.Any(cl => cl.AttributeName.Contains(s)));

            var attributes = await query
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
                .ToPagedList(page, pagesize);

            ViewBag.attributes = processedAttributes;
            ViewBag.s = s;

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            List<string> roles = new List<string>() { UserRole.ADMIN, UserRole.STAFF };
            await authService.CheckRole(roles);

            if (id == null)
            {
                return NotFound();
            }
            var attribute1 = await (from attribute in _context.Attributes
                                    join attributeLanguage in _context.AttributeLanguages
                                    on attribute.AttributeId equals attributeLanguage.AttributeId
                                    where attributeLanguage.Language.Canonical == _currentLanguage && attributeLanguage.AttributeId == id
                                    select new AttributeEditViewModel
                                    {
                                        AttributeCode = attribute.AttributeCode,
                                        AttributeName = attributeLanguage.AttributeName
                                    }).FirstOrDefaultAsync();


            if (attribute1 == null)
            {
                return NotFound();
            }

            return View(attribute1);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            List<string> roles = new List<string>() { UserRole.ADMIN, UserRole.STAFF };
            await authService.CheckRole(roles);

            return View(new AttributeViewModel());
        }


        [HttpPost]
        public async Task<IActionResult> Create(AttributeViewModel avm)
        {
            List<string> roles = new List<string>() { UserRole.ADMIN, UserRole.STAFF };
            await authService.CheckRole(roles);


            if (ModelState.IsValid)
            {
                AttributeC at = new AttributeC();

                at.AttributeCode = avm.AttributeCode;

                _context.Attributes.Add(at);
                await _context.SaveChangesAsync();


                AttributeLanguage atl = new AttributeLanguage();

                atl.Language = _context.Languages.Where(l => l.Canonical == _currentLanguage).FirstOrDefault();
                atl.AttributeName = avm.AttributeName;
                atl.Attribute = at;
                //generate slug 
                atl.Slug = SlugHelper.GenerateSlug(avm.AttributeName);

                _context.AttributeLanguages.Add(atl);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(avm);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            List<string> roles = new List<string>() { UserRole.ADMIN, UserRole.STAFF };
            await authService.CheckRole(roles);

            if (id == null)
            {
                return NotFound();
            }

            var attribute1 = await (from attribute in _context.Attributes
                                    join attributeLanguage in _context.AttributeLanguages
                                    on attribute.AttributeId equals attributeLanguage.AttributeId
                                    where attributeLanguage.Language.Canonical == _currentLanguage && attributeLanguage.AttributeId == id
                                    select new AttributeEditViewModel
                                    {
                                        AttributeCode = attribute.AttributeCode,
                                        AttributeName = attributeLanguage.AttributeName
                                    }).FirstOrDefaultAsync();


            var toView = _mapper.Map<AttributeEditViewModel>(attribute1);

            if (attribute1 == null)
            {
                return NotFound();
            }
            ViewBag.Id = id;

            return View(toView);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, AttributeEditViewModel aevm)
        {
            List<string> roles = new List<string>() { UserRole.ADMIN, UserRole.STAFF };
            await authService.CheckRole(roles);

            var checkExist = await _context.Attributes.FirstOrDefaultAsync(a => a.AttributeId == id);

            if (checkExist != null)
            {
                if (id != checkExist.AttributeId)
                {
                    return NotFound();
                }
            }


            if (ModelState.IsValid)
            {
                try
                {
                    checkExist.AttributeCode = aevm.AttributeCode;

                    _context.Attributes.Update(checkExist);
                    await _context.SaveChangesAsync();

                    var attributeLanguage = await _context.AttributeLanguages.FirstOrDefaultAsync(a => a.AttributeId == id && a.Language.Canonical == _currentLanguage);
                    if (attributeLanguage != null)
                    {

                        attributeLanguage.AttributeName = aevm.AttributeName;
                        attributeLanguage.Slug = SlugHelper.GenerateSlug(aevm.AttributeName);
                        _context.AttributeLanguages.Update(attributeLanguage);
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
            return View(aevm);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            List<string> roles = new List<string>() { UserRole.ADMIN, UserRole.STAFF };
            await authService.CheckRole(roles);

            if (id == null)
            {
                return NotFound();
            }

            var attribute1 = await (from attribute in _context.Attributes
                                    join attributeLanguage in _context.AttributeLanguages
                                    on attribute.AttributeId equals attributeLanguage.AttributeId
                                    where attributeLanguage.Language.Canonical == _currentLanguage && attributeLanguage.AttributeId == id
                                    select new AttributeEditViewModel
                                    {
                                        AttributeCode = attribute.AttributeCode,
                                        AttributeName = attributeLanguage.AttributeName
                                    }).FirstOrDefaultAsync();


            var toView = _mapper.Map<AttributeEditViewModel>(attribute1);

            if (attribute1 == null)
            {
                return NotFound();
            }
            ViewBag.Id = id;

            return View(toView);
        }


        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            List<string> roles = new List<string>() { UserRole.ADMIN, UserRole.STAFF };
            await authService.CheckRole(roles);

            var at = await _context.Attributes.FindAsync(id);
            if (at != null)
            {
                _context.Attributes.Remove(at);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> GetAttributes()
        {
            var attributes = await _context.Attributes
                .Select(a => new
                {
                    Id = a.AttributeId,
                    AttributeName = a.AttributeLanguages.ElementAt(0).AttributeName
                })
                .ToListAsync();

            return Ok(attributes);
        }
    }
}
