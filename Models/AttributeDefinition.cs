namespace DienMayLongQuyen.Api.Models
{
    public class AttributeDefinition
    {
        public int Id { get; set; }

        // Mã nội bộ (VD: "SoCanh", "DungTich")
        public string Name { get; set; }

        // Tên hiển thị (VD: "Số cánh", "Dung tích (L)")
        public string DisplayName { get; set; }

        // Kiểu dữ liệu: "Option" (FK tới bảng Option), "String", "Number"
        public string DataType { get; set; }

        // Thứ tự hiển thị (VD: Dung tích = 1, Số cánh = 2)
        public int DisplayOrder { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<AttributeOption> Options { get; set; }
        public ICollection<ProductAttributeValue> ProductValues { get; set; }
    }
}