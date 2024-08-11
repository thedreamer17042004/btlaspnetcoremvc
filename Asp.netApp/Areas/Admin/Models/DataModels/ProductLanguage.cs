using Asp.netApp.Areas.Admin.Models.DataModel;
using Humanizer;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Asp.netApp.Areas.Admin.Models.DataModels
{
    public class ProductLanguage
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int LanguageId { get; set; }
        public Language Language { get; set; }

        [Required]
        public string ProductName { get; set; }

        public string? Description { get; set; }

        public string Slug { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        [MaxLength(100)]
        public string? CreatedBy { get; set; }

        [MaxLength(100)]
        public string? UpdatedBy { get; set; }
    }
}
