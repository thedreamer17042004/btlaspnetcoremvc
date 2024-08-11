using System.ComponentModel.DataAnnotations;

namespace Asp.netApp.Areas.Admin.Models.ViewModels.Category
{
    public class CategoryViewModel
    {
        [Required(ErrorMessage = "Name không được để trống")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Status không được để trống")]
        public string Status { get; set; }
    }
}
