using Asp.netApp.Areas.Admin.Models.DataModel;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace Asp.netApp.Areas.Admin.Models.DataModels
{
    public class Product
    {
  
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public float Price { get; set; }

        public float SalePrice { get; set; }

        public bool? Publish {  get; set; }

        public string? Image { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [ForeignKey("Brand")]
        public int BrandId { get; set; }
        public Brand Brand { get; set; }

        public string? Album {  get; set; }

        public ICollection<ProductLanguage>? ProductLanguages { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        [MaxLength(100)]
        public string? CreatedBy { get; set; }

        [MaxLength(100)]
        public string? UpdatedBy { get; set; }
        public bool? DeletedAt { get; set; }
    }
}
