using Asp.netApp.Areas.Admin.Models.DataModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Asp.netApp.Areas.Admin.Models.ViewModels.Account
{
    public class AccountViewModel
    {

        [Required(ErrorMessage ="Chưa nhập tên")]      
        public string Username { get; set; }

        [Required(ErrorMessage ="Chưa nhập mật khẩu")]
        public string Password { get; set; }

        [Required(ErrorMessage ="Chưa nhập tên đầu")]
        public string FirstName { get; set; }

        [Required(ErrorMessage ="Chưa nhập tên cuối")]
        public string LastName { get; set; }

        public string? Images { get; set; }

        [EmailAddress]
        [Required(ErrorMessage ="Chưa nhập email")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Chưa chọn giới tính")]
        public int? Gender { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Birthday { get; set; }

        public string? Address { get; set; }

        public string? Phone { get; set; }

        [Required(ErrorMessage = "Chưa chọn Trạng thái")]
        public int? Active { get; set; }

        [Required(ErrorMessage = "Chưa chọn vai trò")]
        public int? RoleId { get; set; }
        public RoleC? Role { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        public string? CreatedBy { get; set; }

        public string? UpdatedBy { get; set; }

        public bool? DeletedAt { get; set; }
    }
}
