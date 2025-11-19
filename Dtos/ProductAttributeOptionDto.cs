using DienMayLongQuyen.Api.Models;

public class ProductAttributeOptionCreateDto
{
    public int ProductId { get; set; }
    public int AttributeOptionId { get; set; }
}


public class ProductAttributeOptionUpdateDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int AttributeOptionId { get; set; }
}

public class ProductAttributeOptionDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int AttributeOptionId { get; set; }
    public string OptionLabel { get; set; }
    public string OptionValueKey { get; set; }
    public string AttributeName { get; set; }
}

    public class AssignOptionToProductsRequest
{
    public int OptionId { get; set; }
    public bool ReplaceSameAttribute { get; set; }
    public int[] ProductIds { get; set; }
}
        
