public class CreateNewsDTO
{
    public string Title { get; set; } = null!;
    public string? Content { get; set; }
    public bool? IsActive { get; set; }
}
public class UpdateNewsDTO
{
    public string? Title { get; set; }
    public string? Content { get; set; }
    public bool? IsActive { get; set; }
}
    