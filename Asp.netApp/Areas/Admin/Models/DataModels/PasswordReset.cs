using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Asp.netApp.Areas.Admin.Models.DataModel
{
    public class PasswordReset
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [MaxLength(36)]
        public string UserId { get; set; }
        public Account User { get; set; }

        [Required]
        [MaxLength(255)]
        public string Token { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? ExpiredAt { get; set; }
    }
}
