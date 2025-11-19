using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DienMayLongQuyen.Api.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(255)]
        public string Slug { get; set; } = string.Empty;

         public string Image { get; set; } = string.Empty;

        public int? ParentId { get; set; }
        public int? IndexShow { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
        public ICollection<BrandCategory> BrandCategories { get; set; }
         public ICollection<AttributeDefinition> Attributes { get; set; }
    }
}

