namespace DienMayLongQuyen.Api.Models
{
    public class AttributeOption
    {
        public int Id { get; set; }

        public int AttributeDefinitionId { get; set; }
        public AttributeDefinition AttributeDefinition { get; set; }

        // Giá trị logic (VD: "2", "4", "CuaNgang")
        public string ValueKey { get; set; }

        // Nhãn hiển thị cho UI (VD: "2 cánh", "4 cánh", "Cửa ngang")
        public string Label { get; set; }

        // Thứ tự hiển thị trong dropdown/filter UI
        public int DisplayOrder { get; set; }

        public ICollection<ProductAttributeOption> ProductAttributeOptions { get; set; }
    }
}