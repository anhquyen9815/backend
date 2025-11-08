public class CreateCategoryDTO
{
    public string Name { get; set; } = null!;
    public string? Slug { get; set; }
    public int? ParentId { get; set; }
    public bool? IsActive { get; set; }
}
