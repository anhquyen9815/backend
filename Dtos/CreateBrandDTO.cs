namespace DienMayLongQuyen.Api.Dtos
{
    public class CreateBrandDTO
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
    }
}
