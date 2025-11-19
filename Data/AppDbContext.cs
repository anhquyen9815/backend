using Microsoft.EntityFrameworkCore;
using DienMayLongQuyen.Api.Models;

namespace DienMayLongQuyen.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // =============================
        // DbSet khai báo các bảng
        // =============================
        public DbSet<Product> Products { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<ProductSpec> ProductSpecs { get; set; }
        public DbSet<BrandCategory> BrandCategories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<AttributeDefinition> AttributeDefinitions { get; set; }
        public DbSet<AttributeOption> AttributeOptions { get; set; }
        public DbSet<ProductAttributeOption> ProductAttributeOptions { get; set; }
        public DbSet<ProductAttributeValue> ProductAttributeValues { get; set; }
        public DbSet<Warranty> Warranties { get; set; }
        public DbSet<ProductWarranty> ProductWarranties { get; set; }



        public static void EnsureTriggers(AppDbContext context)
        {
            var sqlFilePath = Path.Combine(AppContext.BaseDirectory, "Data", "triggers.sql");
            if (!File.Exists(sqlFilePath))
            {
                Console.WriteLine("⚠️ triggers.sql not found, skipping trigger initialization.");
                return;
            }

            var sql = File.ReadAllText(sqlFilePath);
            context.Database.ExecuteSqlRaw(sql);
            Console.WriteLine("✅ All SQLite triggers loaded from triggers.sql");
        }


        // =============================
        // Fluent API cấu hình quan hệ & default values
        // =============================
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            base.OnModelCreating(modelBuilder);

            // ------------------------------------------
            // QUAN HỆ
            // ------------------------------------------

            // Product - Category (1-n)
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Product - Brand (1-n)
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Brand)
                .WithMany(b => b.Products)
                .HasForeignKey(p => p.BrandId)
                .OnDelete(DeleteBehavior.Restrict);

                // Product - Warranty (1-n)
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Warranty)
                .WithMany(w => w.Products)
                .HasForeignKey(p => p.WarrantyId)
                .OnDelete(DeleteBehavior.SetNull); // hoặc Restrict/Cascade theo ý bạn


            // Product - ProductSpec (1-n)
            modelBuilder.Entity<ProductSpec>()
                .HasOne(s => s.Product)
                .WithMany(p => p.Specs)
                .HasForeignKey(s => s.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // ------------------------------------------
            // GIÁ TRỊ MẶC ĐỊNH CHO CreatedAt (SQLite friendly)
            // ------------------------------------------
            modelBuilder.Entity<Product>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("datetime('now')");

            modelBuilder.Entity<Category>()
                .Property(c => c.CreatedAt)
                .HasDefaultValueSql("datetime('now')");

            modelBuilder.Entity<Brand>()
                .Property(b => b.CreatedAt)
                .HasDefaultValueSql("datetime('now')");

            modelBuilder.Entity<News>()
                .Property(n => n.CreatedAt)
                .HasDefaultValueSql("datetime('now')");

            modelBuilder.Entity<ProductSpec>()
                .Property(s => s.CreatedAt)
                .HasDefaultValueSql("datetime('now')");

            modelBuilder.Entity<BrandCategory>()
                .HasIndex(bc => new { bc.BrandId, bc.CategoryId })
                .IsUnique();

            // Quan hệ BrandCategories
            modelBuilder.Entity<BrandCategory>()
                .HasOne(bc => bc.Brand)
                .WithMany(b => b.BrandCategories)
                .HasForeignKey(bc => bc.BrandId);

            modelBuilder.Entity<BrandCategory>()
                .HasOne(bc => bc.Category)
                .WithMany(c => c.BrandCategories)
                .HasForeignKey(bc => bc.CategoryId);

            modelBuilder.Entity<Product>()
                .HasIndex(p => p.Code)
                .IsUnique();

            // Quan hệ ProductImage
            modelBuilder.Entity<ProductImage>()
                .HasOne(pi => pi.Product)
                .WithMany(p => p.Images)
                .HasForeignKey(pi => pi.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Tạo unique index để đảm bảo ProductId + AttributeOptionId là duy nhất
            modelBuilder.Entity<ProductAttributeOption>()
                 .HasIndex(p => new { p.ProductId, p.AttributeOptionId })
                 .IsUnique()
                 .HasDatabaseName("IX_ProductAttributeOptions_ProductId_AttributeOptionId");


            modelBuilder.Entity<ProductWarranty>()
                .HasOne(pw => pw.Product)
                .WithMany(p => p.ProductWarranties)
                .HasForeignKey(pw => pw.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductWarranty>()
                .HasOne(pw => pw.Warranty)
                .WithMany(w => w.ProductWarranties)
                .HasForeignKey(pw => pw.WarrantyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductWarranty>()
                .HasIndex(bc => new { bc.ProductId, bc.WarrantyId })
                .IsUnique();

            modelBuilder.Entity<BrandCategory>()
            .HasIndex(bc => new { bc.BrandId, bc.CategoryId })
            .IsUnique();



        }


    }
}
