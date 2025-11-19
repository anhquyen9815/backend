public class ProductAttributeValueCreateDto
{
    public int ProductId { get; set; }
    public int AttributeDefinitionId { get; set; }
    // Giá trị thực tế (VD: "500L", "1500W")
    public string ValueString { get; set; }
    // Thứ tự hiển thị trong bảng thông số (ưu tiên theo AttributeDefinition.DisplayOrder nếu null)
    public int? DisplayOrder { get; set; }
}

public class ProductAttributeValueUpdateDto
{
    public int ProductId { get; set; }
    public int AttributeDefinitionId { get; set; }
    // Giá trị thực tế (VD: "500L", "1500W")
    public string ValueString { get; set; }
    // Thứ tự hiển thị trong bảng thông số (ưu tiên theo AttributeDefinition.DisplayOrder nếu null)
    public int? DisplayOrder { get; set; }
}
    