using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DienMayLongQuyen.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddDiscountPrecentToProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "DiscountPrecent",
                table: "Products",
                type: "REAL",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountPrecent",
                table: "Products");
        }
    }
}
