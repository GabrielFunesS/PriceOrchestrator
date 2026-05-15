using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceOrchestrator.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPriceChangeRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_product_current_prices_products_ProductId",
                table: "product_current_prices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_products",
                table: "products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_product_current_prices",
                table: "product_current_prices");

            migrationBuilder.RenameTable(
                name: "products",
                newName: "Product");

            migrationBuilder.RenameTable(
                name: "product_current_prices",
                newName: "ProductCurrentPrice");

            migrationBuilder.RenameIndex(
                name: "IX_products_ExternalId",
                table: "Product",
                newName: "IX_Product_ExternalId");

            migrationBuilder.RenameIndex(
                name: "IX_product_current_prices_ProductId",
                table: "ProductCurrentPrice",
                newName: "IX_ProductCurrentPrice_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Product",
                table: "Product",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductCurrentPrice",
                table: "ProductCurrentPrice",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "PriceChangeRequest",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    OldPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    NewPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    EffectiveFromUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AppliedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    RequestedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    RequestSource = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RejectionReason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceChangeRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PriceChangeRequest_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Product_Category_Brand",
                table: "Product",
                columns: new[] { "Category", "Brand" });

            migrationBuilder.CreateIndex(
                name: "IX_PriceChangeRequest_ProductId",
                table: "PriceChangeRequest",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceChangeRequest_Status_EffectiveFromUtc",
                table: "PriceChangeRequest",
                columns: new[] { "Status", "EffectiveFromUtc" });

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCurrentPrice_Product_ProductId",
                table: "ProductCurrentPrice",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCurrentPrice_Product_ProductId",
                table: "ProductCurrentPrice");

            migrationBuilder.DropTable(
                name: "PriceChangeRequest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductCurrentPrice",
                table: "ProductCurrentPrice");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Product",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_Category_Brand",
                table: "Product");

            migrationBuilder.RenameTable(
                name: "ProductCurrentPrice",
                newName: "product_current_prices");

            migrationBuilder.RenameTable(
                name: "Product",
                newName: "products");

            migrationBuilder.RenameIndex(
                name: "IX_ProductCurrentPrice_ProductId",
                table: "product_current_prices",
                newName: "IX_product_current_prices_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Product_ExternalId",
                table: "products",
                newName: "IX_products_ExternalId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_product_current_prices",
                table: "product_current_prices",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_products",
                table: "products",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_product_current_prices_products_ProductId",
                table: "product_current_prices",
                column: "ProductId",
                principalTable: "products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
