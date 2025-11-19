using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DienMayLongQuyen.Api.Migrations
{
    public partial class AddWarrantyId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Thêm cột WarrantyId (nullable) — an toàn, không recreate table
            migrationBuilder.AddColumn<int>(
                name: "WarrantyId",
                table: "Products",
                type: "INTEGER",
                nullable: true);

            // Tùy chọn: tạo index để tìm nhanh khi query/Join
            migrationBuilder.CreateIndex(
                name: "IX_Products_WarrantyId",
                table: "Products",
                column: "WarrantyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_WarrantyId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "WarrantyId",
                table: "Products");
        }
    }
}
