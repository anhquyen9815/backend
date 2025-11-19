using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DienMayLongQuyen.Api.Migrations
{
    /// <inheritdoc />
    public partial class CreateWarrantiesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductWarranty_Products_ProductId",
                table: "ProductWarranty");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductWarranty_Warranty_WarrantyId",
                table: "ProductWarranty");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Warranty",
                table: "Warranty");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductWarranty",
                table: "ProductWarranty");

            migrationBuilder.DropIndex(
                name: "IX_ProductWarranty_ProductId",
                table: "ProductWarranty");

            migrationBuilder.RenameTable(
                name: "Warranty",
                newName: "Warranties");

            migrationBuilder.RenameTable(
                name: "ProductWarranty",
                newName: "ProductWarranties");

            migrationBuilder.RenameIndex(
                name: "IX_ProductWarranty_WarrantyId",
                table: "ProductWarranties",
                newName: "IX_ProductWarranties_WarrantyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Warranties",
                table: "Warranties",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductWarranties",
                table: "ProductWarranties",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProductWarranties_ProductId_WarrantyId",
                table: "ProductWarranties",
                columns: new[] { "ProductId", "WarrantyId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductWarranties_Products_ProductId",
                table: "ProductWarranties",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductWarranties_Warranties_WarrantyId",
                table: "ProductWarranties",
                column: "WarrantyId",
                principalTable: "Warranties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductWarranties_Products_ProductId",
                table: "ProductWarranties");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductWarranties_Warranties_WarrantyId",
                table: "ProductWarranties");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Warranties",
                table: "Warranties");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductWarranties",
                table: "ProductWarranties");

            migrationBuilder.DropIndex(
                name: "IX_ProductWarranties_ProductId_WarrantyId",
                table: "ProductWarranties");

            migrationBuilder.RenameTable(
                name: "Warranties",
                newName: "Warranty");

            migrationBuilder.RenameTable(
                name: "ProductWarranties",
                newName: "ProductWarranty");

            migrationBuilder.RenameIndex(
                name: "IX_ProductWarranties_WarrantyId",
                table: "ProductWarranty",
                newName: "IX_ProductWarranty_WarrantyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Warranty",
                table: "Warranty",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductWarranty",
                table: "ProductWarranty",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProductWarranty_ProductId",
                table: "ProductWarranty",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductWarranty_Products_ProductId",
                table: "ProductWarranty",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductWarranty_Warranty_WarrantyId",
                table: "ProductWarranty",
                column: "WarrantyId",
                principalTable: "Warranty",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
