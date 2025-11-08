namespace DienMayLongQuyen.Api.Dtos
{
    public class CreateNewsDTO
    {
        public string Title { get; set; } = null!;
        public string? Content { get; set; }
        public bool? IsActive { get; set; }
    }
}
