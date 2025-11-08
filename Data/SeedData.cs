using System.Linq;
using DienMayLongQuyen.Api.Models;

namespace DienMayLongQuyen.Api.Data
{
    public static class SeedData
    {
        public static void Initialize(AppDbContext context)
        {
            // if (!context.Categories.Any())
            // {
            //     context.Categories.AddRange(
            //         new Category { Name = "Tivi", Slug = "tivi" },
            //         new Category { Name = "Tủ lạnh", Slug = "tu-lanh" },
            //         new Category { Name = "Máy giặt", Slug = "may-giat" }
            //     );
            //     context.SaveChanges();
            // }

            // if (!context.Brands.Any())
            // {
            //     context.Brands.AddRange(
            //           new Brand { Name = "Samsung", Slug = "samsung", IsActive = true, LogoUrl = "/images/brands/default.png" },
            //           new Brand { Name = "LG", Slug = "lg", IsActive = true, LogoUrl = "/images/brands/default.png" }
            //       );
            //     context.SaveChanges();
            // }
            
            //  if (!context.Products.Any())
            // {
            //   context.Products.AddRange(
            //         new Product { Name = "Samsung", Slug = "samsung", IsActive = true, },
            //         new Product { Name = "LG", Slug = "lg", IsActive = true, },
            //         new Product { Name = "TOSHIBA", Slug = "lg", IsActive = true, }
            //     );
            //     context.SaveChanges();
            // }
        }
    }
}
