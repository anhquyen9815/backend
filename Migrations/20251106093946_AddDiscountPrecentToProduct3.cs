using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DienMayLongQuyen.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddDiscountPrecentToProduct3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DiscountPrecent",
                table: "Products",
                newName: "DiscountPercent");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DiscountPercent",
                table: "Products",
                newName: "DiscountPrecent");
        }
    }
}
