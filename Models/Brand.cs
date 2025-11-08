using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DienMayLongQuyen.Api.Models
{
    public class Brand
    {
        public int Id { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(255)]
        public string Slug { get; set; } = string.Empty;

        [MaxLength(500)]
        public string LogoUrl { get; set; } = string.Empty;  // ✅ thêm dòng này

        public string? Description { get; set; }
        public string? Origin { get; set; }
        public int? IndexShow { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
        public ICollection<BrandCategory> BrandCategories { get; set; }
    }
}
