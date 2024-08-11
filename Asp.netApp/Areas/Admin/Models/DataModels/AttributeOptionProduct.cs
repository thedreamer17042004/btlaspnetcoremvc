using Asp.netApp.Areas.Admin.Models.DataModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Asp.netApp.Areas.Admin.Models.DataModels
{
    public class AttributeOptionProduct
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("AttributeOption")]

        public int AttributeOptionId { get; set; }
        public AttributeOption AttributeOption { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
