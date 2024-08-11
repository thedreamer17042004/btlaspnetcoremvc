using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asp.netApp.Areas.Admin.Models.DataModel
{
    public class Account
    {
        [Key]
        [MaxLength(36)]
        public string AccountId { get; set; }

        [Required]
        [MaxLength(64)]
        public string Username { get; set; }

        [MaxLength(256)]
        public string Password { get; set; }

        [MaxLength(100)]
        public string Fullname { get; set; }

        [MaxLength(100)]
        public string FirstName { get; set; }

        [MaxLength(100)]
        public string LastName { get; set; }

        [MaxLength(512)]
        public string? Images { get; set; }

        [Required]
        [MaxLength(64)]
        public string Email { get; set; }

        public bool? Gender { get; set; }

        public DateTime? Birthday { get; set; }

        [MaxLength(256)]
        public string? Address { get; set; }

        [MaxLength(64)]
        public string? Phone { get; set; }

        public bool Active { get; set; }

        [ForeignKey("Role")]
        public int RoleId { get; set; }
        public RoleC Role { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        [MaxLength(100)]
        public string? CreatedBy { get; set; }

        [MaxLength(100)]
        public string? UpdatedBy { get; set; }

        public bool? DeletedAt { get; set; }
    }
}
