using System.ComponentModel.DataAnnotations;

namespace Asp.netApp.Areas.Admin.Models.ViewModels.AttributeOption
{
    public class AttributeOptionView
    {
        [Required(ErrorMessage ="AttributeId không được để trống")]
        public int AttributeId { get; set; }
        [Required(ErrorMessage = "OptionName không được để trống")]
        public string OptionName { get; set; }
    }
}
