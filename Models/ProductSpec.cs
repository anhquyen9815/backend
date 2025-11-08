using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DienMayLongQuyen.Api.Models
{
    public class ProductSpec
    {
        public int Id { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product? Product { get; set; }

        [Required, MaxLength(255)]
        public string SpecName { get; set; } = string.Empty;

        [Required, MaxLength(255)]
        public string SpecValue { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } 
    }
}
