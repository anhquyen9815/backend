using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DienMayLongQuyen.Api.Migrations
{
    public partial class AddWarrantyFk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Tắt kiểm tra FK cho connection migration (giúp copy dữ liệu an toàn)
            migrationBuilder.Sql("PRAGMA foreign_keys = OFF;");

            // Tạo bảng tạm với FK đến Warranties (và FK tới Brands/Categories như cũ)
            migrationBuilder.CreateTable(
                name: "ef_temp_Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BrandId = table.Column<int>(type: "INTEGER", nullable: true),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: true),
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "datetime('now')"),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Detail = table.Column<string>(type: "TEXT", nullable: false),
                    DiscountPercent = table.Column<int>(type: "INTEGER", nullable: true),
                    DiscountPrice = table.Column<double>(type: "REAL", nullable: true),
                    Gallery = table.Column<string>(type: "TEXT", nullable: false),
                    Image = table.Column<string>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<string>(type: "TEXT", nullable: false),
                    Slug = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
  
                });

            // Copy dữ liệu từ bảng cũ sang bảng tạm (liệt kê đầy đủ cột để tránh lỗi)
            migrationBuilder.Sql(@"
INSERT INTO ""ef_temp_Products"" (Id, BrandId, CategoryId, Code, CreatedAt, Description, Detail, DiscountPercent, DiscountPrice, Gallery, Image, IsActive, Name, Price, Slug)
SELECT Id, BrandId, CategoryId, Code, CreatedAt, Description, Detail, DiscountPercent, DiscountPrice, Gallery, Image, IsActive, Name, Price, Slug
FROM Products;
");

            // Xóa bảng cũ
            migrationBuilder.DropTable(name: "Products");

            // Đổi tên bảng tạm thành Products
            migrationBuilder.RenameTable(name: "ef_temp_Products", newName: "Products");

            // Tạo lại các index (nếu cần). Ví dụ index cho BrandId, CategoryId, WarrantyId:
            migrationBuilder.CreateIndex(
                name: "IX_Products_BrandId",
                table: "Products",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            // Bật lại kiểm tra FK
            migrationBuilder.Sql("PRAGMA foreign_keys = ON;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Reverse: recreate table without FK to Warranties (restore previous shape)
            migrationBuilder.Sql("PRAGMA foreign_keys = OFF;");

            migrationBuilder.CreateTable(
                name: "ef_old_Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BrandId = table.Column<int>(type: "INTEGER", nullable: true),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: true),
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "datetime('now')"),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Detail = table.Column<string>(type: "TEXT", nullable: false),
                    DiscountPercent = table.Column<int>(type: "INTEGER", nullable: true),
                    DiscountPrice = table.Column<double>(type: "REAL", nullable: true),
                    Gallery = table.Column<string>(type: "TEXT", nullable: false),
                    Image = table.Column<string>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<string>(type: "TEXT", nullable: false),
                    Slug = table.Column<string>(type: "TEXT", nullable: false),
                    WarrantyId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    // note: no FK to Warranties in Down() original shape
                });

            migrationBuilder.Sql(@"
INSERT INTO ""ef_old_Products"" (Id, BrandId, CategoryId, Code, CreatedAt, Description, Detail, DiscountPercent, DiscountPrice, Gallery, Image, IsActive, Name, Price, Slug, WarrantyId)
SELECT Id, BrandId, CategoryId, Code, CreatedAt, Description, Detail, DiscountPercent, DiscountPrice, Gallery, Image, IsActive, Name, Price, Slug, WarrantyId
FROM Products;
");

            migrationBuilder.DropTable(name: "Products");
            migrationBuilder.RenameTable(name: "ef_old_Products", newName: "Products");

            migrationBuilder.CreateIndex(
                name: "IX_Products_BrandId",
                table: "Products",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.Sql("PRAGMA foreign_keys = ON;");
        }
    }
}
