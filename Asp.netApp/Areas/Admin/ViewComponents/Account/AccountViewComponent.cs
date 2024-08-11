using Asp.netApp.Areas.Admin.Data;
using Asp.netApp.Areas.Admin.Models.DataModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Asp.netApp.Areas.Admin.ViewComponents.Account
{
    public class AccountViewComponent:ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public AccountViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(string viewType = "Default", bool btnShow= false)
        {
            if (viewType == "Detail")
            {
                ViewData["btnCheck"] = btnShow;
                // Logic for the detailed view
                return View("Detail");
            }

            if(viewType == "Create")
            {
                return View("Create");
            }
            if (viewType == "Edit")
            {
                return View("Edit");
            }
            if(viewType == "FormSearch")
            {
                return View("FormSearch");
            }
            // Default logic
            return View("Default");
        }
    }
}
