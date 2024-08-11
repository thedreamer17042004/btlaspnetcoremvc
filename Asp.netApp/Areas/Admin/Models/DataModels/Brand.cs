using Asp.netApp.Areas.Admin.Models.DataModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asp.netApp.Areas.Admin.Models.DataModel
{
    public class Brand
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BrandId { get; set; }

        [Required]
        [MaxLength(255)]
        public string BrandName { get; set; }

        public bool Active { get; set; }

        [MaxLength(255)]
        public string Slug { get; set; }

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
