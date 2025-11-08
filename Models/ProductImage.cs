using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DienMayLongQuyen.Api.Models
{
    public class ProductImage
    {
        public int Id { get; set; }

        [Required]
        public string ImageUrl { get; set; } = string.Empty;

        public string? AltText { get; set; } // mô tả ảnh (tùy chọn)
        public int SortOrder { get; set; } = 0; // để sắp xếp thứ tự hiển thị nếu cần

        // ====== Khóa ngoại ======
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
    }
}
