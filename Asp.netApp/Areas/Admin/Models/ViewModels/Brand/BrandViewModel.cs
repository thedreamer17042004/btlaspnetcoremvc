using System.ComponentModel.DataAnnotations;

namespace Asp.netApp.Areas.Admin.Models.ViewModels.Brand
{
    public class BrandViewModel
    {
        [Required(ErrorMessage ="BrandName không được để trống")]
        public string BrandName { get; set; }
        [Required(ErrorMessage = "Active không được để trống")]
        public string? Active {  get; set; }
    }
}
