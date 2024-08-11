using Asp.netApp.Areas.Admin.Models.DataModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asp.netApp.Areas.Admin.Models.DataModel
{
    public class AttributeC
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AttributeId { get; set; }

        [MaxLength(200)]
        [Required]
        public string AttributeCode { get; set; }

        public ICollection<AttributeLanguage>? AttributeLanguages { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        [MaxLength(100)]
        public string? CreatedBy { get; set; }

        [MaxLength(100)]
        public string? UpdatedBy { get; set; }
    }
}
