using Asp.netApp.Areas.Admin.Models.DataModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;


namespace Asp.netApp.Areas.Admin.Models.ViewModels.Product
{
    public class ProductEditViewModel
    {
        
        public int? Id { get; set; }
        [Required(ErrorMessage = "Price Chưa nhập")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Gia phai la so dương")]
        public float Price { get; set; }

        public float? SalePrice { get; set; }
        [Required(ErrorMessage ="Publish chưa chọn")]
        public string Publish { get; set; }

        public string? Image { get; set; }
        [Required(ErrorMessage ="CategoryId chưa chọn")]
        public int? CategoryId { get; set; }
        public string? Category { get; set; }

        [Required(ErrorMessage ="Brand chưa chọn")]
        public int? BrandId { get; set; }
        public Asp.netApp.Areas.Admin.Models.DataModel.Brand? Brand { get; set; }

        public string? Album { get; set; }

        [Required(ErrorMessage ="Product Name chưa nhập")]
        public string ProductName { get; set; }

        public string Description { get; set; }
        
    }
}
