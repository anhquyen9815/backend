// namespace DienMayLongQuyen.Api.Dtos
// {
//     public class CreateProductDTO
//     {
//        public string? Name { get; set; }
//         public string? Code { get; set; }
//         public string? Slug { get; set; }
//         public string? Description { get; set; }
//         public decimal? Price { get; set; }
//         public double? DiscountPrice { get; set; }
//         public int? DiscountPercent { get; set; }
//         public int? CategoryId { get; set; }
//         public int? BrandId { get; set; }
//         public string? Image { get; set; }
//         public bool? IsActive { get; set; }
//     }
// }

public class CreateProductDTO
{
    public string? Name { get; set; }
    public string? Code { get; set; }
    public string? Slug { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public double? DiscountPrice { get; set; }
    public int? DiscountPercent { get; set; }
    public int? CategoryId { get; set; }
    public int? BrandId { get; set; }
    //  public int? WarrantyId { get; set; }
    public string? Image { get; set; }
    public string? Gallery { get; set; }
    public bool? IsActive { get; set; }
}

public class UpdateProductDTO
{
    public string? Name { get; set; }
    public string? Code { get; set; }
    public string? Slug { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public double? DiscountPrice { get; set; }
    public int? DiscountPercent { get; set; }
    public int? CategoryId { get; set; }
    public int? BrandId { get; set; }
    // public int? WarrantyId { get; set; }
    
    public string? Image { get; set; }
    public string? Gallery { get; set; }
    public bool? IsActive { get; set; }
}

public class UpdateProductGalleryDTO
{

    public int? IdProduct { get; set; }
    public string? CodeProduct { get; set; }
    public string? Image { get; set; }
    public string? Gallery { get; set; }
}