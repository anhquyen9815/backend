using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DienMayLongQuyen.Api.Migrations
{
    /// <inheritdoc />
    public partial class RemoveWarrantyId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Turn off FK checks for safety
            migrationBuilder.Sql("PRAGMA foreign_keys = OFF;");

            // Create temporary table WITHOUT WarrantyId
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
                    // NOTE: intentionally no WarrantyId here
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

            // Copy data from old Products to ef_temp_Products (list columns explicitly)
            migrationBuilder.Sql(@"
INSERT INTO ""ef_temp_Products"" (Id, BrandId, CategoryId, Code, CreatedAt, Description, Detail, DiscountPercent, DiscountPrice, Gallery, Image, IsActive, Name, Price, Slug)
SELECT Id, BrandId, CategoryId, Code, CreatedAt, Description, Detail, DiscountPercent, DiscountPrice, Gallery, Image, IsActive, Name, Price, Slug
FROM Products;
");

            // Drop old table and rename temp to Products
            migrationBuilder.DropTable(name: "Products");
            migrationBuilder.RenameTable(name: "ef_temp_Products", newName: "Products");

            // Recreate indexes you need
            migrationBuilder.CreateIndex(
                name: "IX_Products_BrandId",
                table: "Products",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            // Re-enable FK checks
            migrationBuilder.Sql("PRAGMA foreign_keys = ON;");
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
