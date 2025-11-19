public class CreateCategoryDTO
{
    public string Name { get; set; } = null!;
    public string? Slug { get; set; }
    public int? ParentId { get; set; }
    public bool? IsActive { get; set; }
}

public class UpdateCategoryDTO
{
    public string? Name { get; set; }
    public string? Slug { get; set; }
    public int? ParentId { get; set; }
    public bool? IsActive { get; set; }
}
