using Asp.netApp.Areas.Admin.Models.DataModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Asp.netApp.Areas.Admin.Models.ViewModels.Category
{
    public class CategoryEditViewModel
    {
        public string? Picture { get; set; }

        public string? Active { get; set; }
        [Required(ErrorMessage ="CategoryName chưa nhập")]
        public string CategoryName { get; set; }
    }
}
