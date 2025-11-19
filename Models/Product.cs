using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DienMayLongQuyen.Api.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;

        public string Slug { get; set; } = string.Empty;

        [ForeignKey("Category")]
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        [ForeignKey("Brand")]
        public int? BrandId { get; set; }
        public Brand? Brand { get; set; }

        [ForeignKey("Warranty")]
        public int? WarrantyId { get; set; }
        public Warranty? Warranty { get; set; }

        public decimal Price { get; set; }
        public double? DiscountPrice { get; set; }
        public int? DiscountPercent { get; set; }
        public string? Description { get; set; } = string.Empty;
        public string Detail { get; set; } = string.Empty;

        public string Image { get; set; } = string.Empty;
        public string Gallery { get; set; } = string.Empty;
        //  public List<string> Gallery { get; set; } = new();

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }

        public ICollection<ProductSpec> Specs { get; set; } = new List<ProductSpec>();
        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();

        public ICollection<ProductAttributeOption> ProductAttributeOptions { get; set; }
        public ICollection<ProductAttributeValue> ProductAttributeValues { get; set; }
        // public ICollection<ProductWarranty> ProductWarranties { get; set; } = new List<ProductWarranty>();


        // Nếu bạn có dùng ở controller product.ImageUrl thì thêm phần này:
        [NotMapped]
        public string ImageUrl => $"/uploads/products/{Image}";
    }
}
