using System.ComponentModel.DataAnnotations;

namespace Asp.netApp.Areas.Admin.Models.ViewModels.AttributeOption
{
    public class AttributeOptionEditViewModel
    {
        public int AttId { get; set; }
        [Required(ErrorMessage = "OptionName không được để trống")]
        public string OptionName { get; set; }
    }
}
