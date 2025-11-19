using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DienMayLongQuyen.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddWarrantyRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WarrantyId",
                table: "Products",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_WarrantyId",
                table: "Products",
                column: "WarrantyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Warranties_WarrantyId",
                table: "Products",
                column: "WarrantyId",
                principalTable: "Warranties",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Warranties_WarrantyId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_WarrantyId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "WarrantyId",
                table: "Products");
        }
    }
}
