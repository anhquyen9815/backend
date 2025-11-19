// Models/AttributeDefinitionDto.cs
public class AttributeDefinitionDto
{
    public int Id { get; set; }
    public string Name { get; set; }         // key, ex: "SoCanh"
    public string DisplayName { get; set; }  // "Số cánh"
    public string DataType { get; set; }     // "Option" | "String" | "Number"
    public int DisplayOrder { get; set; }
    public int CategoryId { get; set; }
}
    
public class AttributeDefinitionCreateDto
{
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public string DataType { get; set; } // prefer "Option","String","Number"
    public int DisplayOrder { get; set; } = 0;
    public int CategoryId { get; set; }
}

public class AttributeDefinitionUpdateDto
{
    public string DisplayName { get; set; }
    public string DataType { get; set; }
    public int? DisplayOrder { get; set; }
    public int? CategoryId { get; set; }
}
