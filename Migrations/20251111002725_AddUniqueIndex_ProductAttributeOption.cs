using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DienMayLongQuyen.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndex_ProductAttributeOption : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductAttributeOptions_ProductId",
                table: "ProductAttributeOptions");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttributeOptions_ProductId_AttributeOptionId",
                table: "ProductAttributeOptions",
                columns: new[] { "ProductId", "AttributeOptionId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductAttributeOptions_ProductId_AttributeOptionId",
                table: "ProductAttributeOptions");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttributeOptions_ProductId",
                table: "ProductAttributeOptions",
                column: "ProductId");
        }
    }
}
