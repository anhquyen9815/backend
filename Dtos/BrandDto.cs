public class CreateBrandDTO
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool? IsActive { get; set; }
}

public class UpdateBrandDTO
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool? IsActive { get; set; }
}
    