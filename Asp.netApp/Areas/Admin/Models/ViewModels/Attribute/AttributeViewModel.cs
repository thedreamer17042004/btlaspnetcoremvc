using System.ComponentModel.DataAnnotations;

namespace Asp.netApp.Areas.Admin.Models.ViewModels.Attribute
{
    public class AttributeViewModel
    {
        [Required(ErrorMessage = "Attribute code không được để trống")]
        public string AttributeCode { get; set; }

        [Required(ErrorMessage = "Attribute không được để trống")]
        public string AttributeName { get; set; }
    }
}
