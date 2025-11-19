public class AttributeOptionDto
{
    public int Id { get; set; }
    public int AttributeDefinitionId { get; set; }
    public string ValueKey { get; set; }   // logic key, ex: "4", "CuaNgang"
    public string Label { get; set; }      // display: "4 cánh", "Cửa ngang"
    public int DisplayOrder { get; set; }
}

public class AttributeOptionCreateDto
{
    public int AttributeDefinitionId { get; set; }
    public string ValueKey { get; set; }
    public string Label { get; set; }
    public int DisplayOrder { get; set; } = 0;
}

public class AttributeOptionUpdateDto
{
    public string ValueKey { get; set; }   // optional: allow updating key if you want (careful)
    public string Label { get; set; }
    public int? DisplayOrder { get; set; }
}
