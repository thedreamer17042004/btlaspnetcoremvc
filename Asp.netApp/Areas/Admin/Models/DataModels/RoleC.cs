using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asp.netApp.Areas.Admin.Models.DataModel
{

    public class RoleC
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleId { get; set; }

        [Required]
        public string RoleName { get; set; }

        [Required]
        public string Permissions { get; set; }

        public bool Active { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        [MaxLength(100)]
        public string? UpdatedBy { get; set; }

        [MaxLength(100)]
        public string? CreatedBy { get; set; }
        public bool? DeletedAt { get; set; }

    }
}