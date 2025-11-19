using System.ComponentModel.DataAnnotations;

namespace DienMayLongQuyen.Api.Models
{
    public class Warranty
    {
        public int Id { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Description { get; set; } = string.Empty;

         public int PeriodMonths { get; set; } 

        public int? IndexShow { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<ProductWarranty> ProductWarranties { get; set; } = new List<ProductWarranty>();
        public ICollection<Product> Products { get; set; } = new List<Product>();

    }
}
