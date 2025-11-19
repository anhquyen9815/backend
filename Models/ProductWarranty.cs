// using System;
// using System.ComponentModel.DataAnnotations;
// using System.ComponentModel.DataAnnotations.Schema;

// namespace DienMayLongQuyen.Api.Models
// {
//     public class ProductWarranty
//     {
//         public int Id { get; set; }

//         // FK tới Product
//         public int ProductId { get; set; }
//         public Product? Product { get; set; }

//         // FK tới Warranty
//         // public int WarrantyId { get; set; }
//         // public Warranty? Warranty { get; set; }

//         // Thông tin tuỳ chọn cho quan hệ này:
//         public string? WarrantyNumber { get; set; }      // mã bảo hành (nếu cần)
//         public DateTime? StartDate { get; set; }
//         public DateTime? EndDate { get; set; }
//         public int? PeriodMonths { get; set; }           // có thể override PeriodMonths từ Warranty
//         public decimal? Price { get; set; }              // nếu bán gói bảo hành
//         public string? Notes { get; set; }

//         public bool IsDeleted { get; set; } = false;
//         public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
//         public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
//     }
// }
