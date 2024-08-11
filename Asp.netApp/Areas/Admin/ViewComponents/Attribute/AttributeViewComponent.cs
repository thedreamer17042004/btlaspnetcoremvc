using Asp.netApp.Areas.Admin.Data;
using Microsoft.AspNetCore.Mvc;

namespace Asp.netApp.Areas.Admin.ViewComponents.Attribute
{
    public class AttributeViewComponent:ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public AttributeViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(string viewType = "Default", bool btnShow = false)
        {
            if (viewType == "Detail")
            {
                ViewData["btnCheck"] = btnShow;
                // Logic for the detailed view
                return View("Detail");
            }

            if (viewType == "Create")
            {
                return View("Create");
            }
            if (viewType == "Edit")
            {
                return View("Edit");
            }
            if (viewType == "FormSearch")
            {
                return View("FormSearch");
            }
            // Default logic
            return View("Default");
        }
    }
}
