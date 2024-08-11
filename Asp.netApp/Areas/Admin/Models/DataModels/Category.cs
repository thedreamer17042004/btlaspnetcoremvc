using Asp.netApp.Areas.Admin.Models.DataModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asp.netApp.Areas.Admin.Models.DataModel
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? Picture { get; set; }

        public bool Active { get; set; }

        public int? ParentId { get; set; }

        public ICollection<CategoryLanguage> CategoryLanguages { get; set; }
        public ICollection<Product>? Products { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        [MaxLength(100)]
        public string? CreatedBy { get; set; }

        [MaxLength(100)]
        public string? UpdatedBy { get; set; }
        public bool? DeletedAt { get; set; }
    }
}
