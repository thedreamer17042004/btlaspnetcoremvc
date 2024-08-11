using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Asp.netApp.Areas.Admin.Models.DataModel;
namespace Asp.netApp.Areas.Admin.Models.DataModels
{
    public class AttributeOption
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Attribute")]
        public int AttributeId { get; set; }
        public AttributeC Attribute { get; set; }
        public ICollection<AttributeOptionLanguage>? AttributeOptionLanguage { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        [MaxLength(100)]
        public string? CreatedBy { get; set; }

        [MaxLength(100)]
        public string? UpdatedBy { get; set; }
    }
}
