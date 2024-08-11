using Asp.netApp.Areas.Admin.Common;
using Asp.netApp.Areas.Admin.Constants;
using Asp.netApp.Areas.Admin.Data;
using Asp.netApp.Areas.Admin.Models.DataModel;
using Asp.netApp.Areas.Admin.Models.DataModels;
using Asp.netApp.Areas.Admin.Models.ViewModels.Attribute;
using Asp.netApp.Areas.Admin.Services.Auth;
using Asp.netApp.Areas.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using Microsoft.EntityFrameworkCore;
using Asp.netApp.Areas.Admin.Models.ViewModels.AttributeOption;

namespace Asp.netApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AttributeOptionController : Controller
    {
        private readonly AuthService authService;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly LanguageService _languageService;
        private readonly string _currentLanguage;
        private readonly ILogger<AttributeOptionController> _logger;
        public AttributeOptionController(ApplicationDbContext db, AuthService authService, IMapper mapper, LanguageService languageService, ILogger<AttributeOptionController> logger)
        {
            this._context = db;
            this.authService = authService;
            this._mapper = mapper;
            this._logger = logger;
            _currentLanguage = languageService.GetCurrentLanguage();
        }
        [HttpGet]
        public async Task<IActionResult> Index(int attributeId, string s, int page = 1)
        {
            List<string> roles = new List<string>() { UserRole.ADMIN };
            await authService.CheckRole(roles);

            int pagesize = 10;

            s = string.IsNullOrEmpty(s) ? "" : s;


            var query = _context.AttributeOptions.Include(c => c.AttributeOptionLanguage).ThenInclude(z => z.Language).AsQueryable();
            
            if (attributeId != null)
            {
                query = query.Where(c => c.AttributeId.Equals(attributeId));

            }

            query = query.Where(c => c.AttributeOptionLanguage.Any(cl => cl.OptionName.Contains(s)));

            var attributeOptions = await query
               .Select(c => new
               {
                   AttributeOption = c,
                   AttributeOptionLanguage = c.AttributeOptionLanguage
                       .FirstOrDefault(cl => cl.Language.Canonical.Equals(_currentLanguage))
               })
               .ToListAsync();

            var processedAttributes = attributeOptions
                .Where(c => c.AttributeOptionLanguage != null)
                .Select(c =>
                {
                    c.AttributeOption.AttributeOptionLanguage = new List<AttributeOptionLanguage> { c.AttributeOptionLanguage };
                    return c.AttributeOption;
                })
                .ToPagedList(page, pagesize);

            ViewBag.attributeOptions = processedAttributes;
            ViewBag.s = s;
            ViewBag.attributeId = attributeId;
            
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
            var attributeOptions = await (from attributeOption in _context.AttributeOptions
                                          join attributeOptionLanguage in _context.AttributeOptionLanguages
                                          on attributeOption.Id equals attributeOptionLanguage.AttributeOptionId
                                          where attributeOptionLanguage.Language.Canonical == _currentLanguage
                                          && attributeOption.Id == id
                                          select new AttributeOptionEditViewModel
                                          {
                                              AttId = attributeOption.AttributeId,
                                              OptionName = attributeOptionLanguage.OptionName

                                          }).FirstOrDefaultAsync();


            if (attributeOptions == null)
            {
                return NotFound();
            }
            ViewBag.attributeId = attributeOptions.AttId;
            return View(attributeOptions);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int attributeId)
        {
            List<string> roles = new List<string>() { UserRole.ADMIN };
            await authService.CheckRole(roles);

            ViewBag.attributeId = attributeId;

            return View(new AttributeOptionView());
        }


        [HttpPost]
        public async Task<IActionResult> Create(int attributeId, AttributeOptionView aov)
        {
            List<string> roles = new List<string>() { UserRole.ADMIN };
            await authService.CheckRole(roles);


            if (ModelState.IsValid)
            {
                AttributeOption ao = new AttributeOption();

                ao.Attribute = await _context.Attributes.FindAsync(aov.AttributeId);

                _context.AttributeOptions.Add(ao);
                await _context.SaveChangesAsync();


                AttributeOptionLanguage aol = new AttributeOptionLanguage();

                aol.Language = _context.Languages.Where(l => l.Canonical == _currentLanguage).FirstOrDefault();
                aol.OptionName = aov.OptionName;
                aol.AttributeOption = ao;
                //generate slug 
                aol.Slug = SlugHelper.GenerateSlug(aov.OptionName);

                _context.AttributeOptionLanguages.Add(aol);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index), new {attributeId = attributeId});
            }
            ViewBag.attributeId = attributeId;
            return View(aov);
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

            var attributeOptions = await (from attributeOption in _context.AttributeOptions
                                          join attributeOptionLanguage in _context.AttributeOptionLanguages
                                          on attributeOption.Id equals attributeOptionLanguage.AttributeOptionId
                                          where attributeOptionLanguage.Language.Canonical == _currentLanguage
                                            && attributeOption.Id == id
                                          select new AttributeOptionEditViewModel
                                          {
                                              AttId = attributeOption.AttributeId,
                                              OptionName = attributeOptionLanguage.OptionName

                                          }).FirstOrDefaultAsync();

            var toView = _mapper.Map<AttributeOptionEditViewModel>(attributeOptions);

            if (attributeOptions == null)
            {
                return NotFound();
            }
            ViewBag.Id = id;
            ViewBag.attributeId = attributeOptions.AttId;
            return View(toView);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, AttributeOptionEditViewModel aevm, int attributeId)
        {
            List<string> roles = new List<string>() { UserRole.ADMIN };
            await authService.CheckRole(roles);

            var checkExist = await _context.AttributeOptions.FirstOrDefaultAsync(a => a.AttributeId == id);

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
                    var attributeOptionLanguage = await _context.AttributeOptionLanguages.FirstOrDefaultAsync(a => a.AttributeOptionId == id && a.Language.Canonical == _currentLanguage);
                    if (attributeOptionLanguage != null)
                    {

                        attributeOptionLanguage.OptionName = aevm.OptionName;
                        attributeOptionLanguage.Slug = SlugHelper.GenerateSlug(aevm.OptionName);
                        _context.AttributeOptionLanguages.Update(attributeOptionLanguage);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;

                }
                return RedirectToAction(nameof(Index) , new {attributeId = attributeId });
            }

            ViewBag.Id = id;
            ViewBag.attributeId = attributeId;
            return View(aevm);
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

            var attributeOptions = await (from attributeOption in _context.AttributeOptions
                                          join attributeOptionLanguage in _context.AttributeOptionLanguages
                                          on attributeOption.Id equals attributeOptionLanguage.AttributeOptionId
                                          where attributeOptionLanguage.Language.Canonical == _currentLanguage 
                                          && attributeOption.Id == id
                                          select new AttributeOptionEditViewModel
                                          {
                                              AttId = attributeOption.AttributeId,
                                              OptionName = attributeOptionLanguage.OptionName

                                          }).FirstOrDefaultAsync();

            var toView = _mapper.Map<AttributeOptionEditViewModel>(attributeOptions);

            if (attributeOptions == null)
            {
                return NotFound();
            }
            ViewBag.Id = id;
            ViewBag.attributeId = attributeOptions.AttId;

            return View(toView);
        }


        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id, int attributeId)
        {
            List<string> roles = new List<string>() { UserRole.ADMIN };
            await authService.CheckRole(roles);

            var at = await _context.AttributeOptions.FindAsync(id);
            if (at != null)
            {
                _context.AttributeOptions.Remove(at);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new {attributeId= attributeId});
        }

        [HttpGet]
        public async Task<IActionResult> GetOptions([FromQuery] int attributeId)
        {
            var result = await _context.AttributeOptions
              .Include(o => o.Attribute)
              .Where(a => a.AttributeId == attributeId)
              .Select(o => new
              {
                  Id = o.Id,
                  OptionName = o.AttributeOptionLanguage.ElementAt(0).OptionName
              })
              .ToListAsync();
              return Ok(result);
        }
          
        
    }
}
