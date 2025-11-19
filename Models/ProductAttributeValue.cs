namespace DienMayLongQuyen.Api.Models
{
    public class ProductAttributeValue
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int AttributeDefinitionId { get; set; }
        public AttributeDefinition AttributeDefinition { get; set; }

        // Giá trị thực tế (VD: "500L", "1500W")
        public string ValueString { get; set; }

        // Thứ tự hiển thị trong bảng thông số (ưu tiên theo AttributeDefinition.DisplayOrder nếu null)
        public int? DisplayOrder { get; set; }
    }
}