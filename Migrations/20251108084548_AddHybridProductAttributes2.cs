using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DienMayLongQuyen.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddHybridProductAttributes2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttributeDefinition_Categories_CategoryId",
                table: "AttributeDefinition");

            migrationBuilder.DropForeignKey(
                name: "FK_AttributeOption_AttributeDefinition_AttributeDefinitionId",
                table: "AttributeOption");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductAttributeOption_AttributeOption_AttributeOptionId",
                table: "ProductAttributeOption");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductAttributeOption_Products_ProductId",
                table: "ProductAttributeOption");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductAttributeValue_AttributeDefinition_AttributeDefinitionId",
                table: "ProductAttributeValue");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductAttributeValue_Products_ProductId",
                table: "ProductAttributeValue");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductAttributeValue",
                table: "ProductAttributeValue");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductAttributeOption",
                table: "ProductAttributeOption");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AttributeOption",
                table: "AttributeOption");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AttributeDefinition",
                table: "AttributeDefinition");

            migrationBuilder.RenameTable(
                name: "ProductAttributeValue",
                newName: "ProductAttributeValues");

            migrationBuilder.RenameTable(
                name: "ProductAttributeOption",
                newName: "ProductAttributeOptions");

            migrationBuilder.RenameTable(
                name: "AttributeOption",
                newName: "AttributeOptions");

            migrationBuilder.RenameTable(
                name: "AttributeDefinition",
                newName: "AttributeDefinitions");

            migrationBuilder.RenameIndex(
                name: "IX_ProductAttributeValue_ProductId",
                table: "ProductAttributeValues",
                newName: "IX_ProductAttributeValues_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductAttributeValue_AttributeDefinitionId",
                table: "ProductAttributeValues",
                newName: "IX_ProductAttributeValues_AttributeDefinitionId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductAttributeOption_ProductId",
                table: "ProductAttributeOptions",
                newName: "IX_ProductAttributeOptions_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductAttributeOption_AttributeOptionId",
                table: "ProductAttributeOptions",
                newName: "IX_ProductAttributeOptions_AttributeOptionId");

            migrationBuilder.RenameIndex(
                name: "IX_AttributeOption_AttributeDefinitionId",
                table: "AttributeOptions",
                newName: "IX_AttributeOptions_AttributeDefinitionId");

            migrationBuilder.RenameIndex(
                name: "IX_AttributeDefinition_CategoryId",
                table: "AttributeDefinitions",
                newName: "IX_AttributeDefinitions_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductAttributeValues",
                table: "ProductAttributeValues",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductAttributeOptions",
                table: "ProductAttributeOptions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AttributeOptions",
                table: "AttributeOptions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AttributeDefinitions",
                table: "AttributeDefinitions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AttributeDefinitions_Categories_CategoryId",
                table: "AttributeDefinitions",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AttributeOptions_AttributeDefinitions_AttributeDefinitionId",
                table: "AttributeOptions",
                column: "AttributeDefinitionId",
                principalTable: "AttributeDefinitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAttributeOptions_AttributeOptions_AttributeOptionId",
                table: "ProductAttributeOptions",
                column: "AttributeOptionId",
                principalTable: "AttributeOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAttributeOptions_Products_ProductId",
                table: "ProductAttributeOptions",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAttributeValues_AttributeDefinitions_AttributeDefinitionId",
                table: "ProductAttributeValues",
                column: "AttributeDefinitionId",
                principalTable: "AttributeDefinitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAttributeValues_Products_ProductId",
                table: "ProductAttributeValues",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttributeDefinitions_Categories_CategoryId",
                table: "AttributeDefinitions");

            migrationBuilder.DropForeignKey(
                name: "FK_AttributeOptions_AttributeDefinitions_AttributeDefinitionId",
                table: "AttributeOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductAttributeOptions_AttributeOptions_AttributeOptionId",
                table: "ProductAttributeOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductAttributeOptions_Products_ProductId",
                table: "ProductAttributeOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductAttributeValues_AttributeDefinitions_AttributeDefinitionId",
                table: "ProductAttributeValues");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductAttributeValues_Products_ProductId",
                table: "ProductAttributeValues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductAttributeValues",
                table: "ProductAttributeValues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductAttributeOptions",
                table: "ProductAttributeOptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AttributeOptions",
                table: "AttributeOptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AttributeDefinitions",
                table: "AttributeDefinitions");

            migrationBuilder.RenameTable(
                name: "ProductAttributeValues",
                newName: "ProductAttributeValue");

            migrationBuilder.RenameTable(
                name: "ProductAttributeOptions",
                newName: "ProductAttributeOption");

            migrationBuilder.RenameTable(
                name: "AttributeOptions",
                newName: "AttributeOption");

            migrationBuilder.RenameTable(
                name: "AttributeDefinitions",
                newName: "AttributeDefinition");

            migrationBuilder.RenameIndex(
                name: "IX_ProductAttributeValues_ProductId",
                table: "ProductAttributeValue",
                newName: "IX_ProductAttributeValue_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductAttributeValues_AttributeDefinitionId",
                table: "ProductAttributeValue",
                newName: "IX_ProductAttributeValue_AttributeDefinitionId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductAttributeOptions_ProductId",
                table: "ProductAttributeOption",
                newName: "IX_ProductAttributeOption_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductAttributeOptions_AttributeOptionId",
                table: "ProductAttributeOption",
                newName: "IX_ProductAttributeOption_AttributeOptionId");

            migrationBuilder.RenameIndex(
                name: "IX_AttributeOptions_AttributeDefinitionId",
                table: "AttributeOption",
                newName: "IX_AttributeOption_AttributeDefinitionId");

            migrationBuilder.RenameIndex(
                name: "IX_AttributeDefinitions_CategoryId",
                table: "AttributeDefinition",
                newName: "IX_AttributeDefinition_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductAttributeValue",
                table: "ProductAttributeValue",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductAttributeOption",
                table: "ProductAttributeOption",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AttributeOption",
                table: "AttributeOption",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AttributeDefinition",
                table: "AttributeDefinition",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AttributeDefinition_Categories_CategoryId",
                table: "AttributeDefinition",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AttributeOption_AttributeDefinition_AttributeDefinitionId",
                table: "AttributeOption",
                column: "AttributeDefinitionId",
                principalTable: "AttributeDefinition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAttributeOption_AttributeOption_AttributeOptionId",
                table: "ProductAttributeOption",
                column: "AttributeOptionId",
                principalTable: "AttributeOption",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAttributeOption_Products_ProductId",
                table: "ProductAttributeOption",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAttributeValue_AttributeDefinition_AttributeDefinitionId",
                table: "ProductAttributeValue",
                column: "AttributeDefinitionId",
                principalTable: "AttributeDefinition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAttributeValue_Products_ProductId",
                table: "ProductAttributeValue",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
