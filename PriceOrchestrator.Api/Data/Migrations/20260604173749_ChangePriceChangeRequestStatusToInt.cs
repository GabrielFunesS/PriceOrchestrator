using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceOrchestrator.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangePriceChangeRequestStatusToInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Keep LastPriceChangeRequestId nullable to avoid forcing a repeated default Guid
            // which would break unique indexes if multiple rows are present.
            migrationBuilder.AlterColumn<Guid>(
                name: "LastPriceChangeRequestId",
                table: "ProductCurrentPrice",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InvalidationReason",
                table: "Product",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Product",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            // Convert existing textual status values to their integer equivalents
            // then alter the column type to integer using a cast.
            migrationBuilder.Sql(@"
UPDATE ""PriceChangeRequest""
SET ""Status"" = CASE ""Status""
  WHEN 'Pending' THEN '1'
  WHEN 'Applied' THEN '2'
  WHEN 'Expired' THEN '3'
  WHEN 'Cancelled' THEN '4'
  ELSE '1' END;
");

            migrationBuilder.Sql(@"ALTER TABLE ""PriceChangeRequest"" ALTER COLUMN ""Status"" TYPE integer USING (""Status""::integer);");

            migrationBuilder.AlterColumn<string>(
                name: "RejectionReason",
                table: "PriceChangeRequest",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            // Create a unique index only for non-null LastPriceChangeRequestId values to
            // avoid conflicts when multiple rows have NULL. Use raw SQL for a partial index
            // since EF Core CreateIndex may not emit the WHERE clause for all providers.
            migrationBuilder.Sql(@"CREATE UNIQUE INDEX IF NOT EXISTS ""IX_ProductCurrentPrice_LastPriceChangeRequestId"" ON ""ProductCurrentPrice""(""LastPriceChangeRequestId"") WHERE ""LastPriceChangeRequestId"" IS NOT NULL;");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCurrentPrice_PriceChangeRequest_LastPriceChangeRequestId",
                table: "ProductCurrentPrice",
                column: "LastPriceChangeRequestId",
                principalTable: "PriceChangeRequest",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop the FK we added in Up
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCurrentPrice_PriceChangeRequest_LastPriceChangeRequestId",
                table: "ProductCurrentPrice");

            // Drop the partial unique index created via raw SQL in Up
            migrationBuilder.Sql(@"DROP INDEX IF EXISTS ""IX_ProductCurrentPrice_LastPriceChangeRequestId"";");

            migrationBuilder.DropColumn(
                name: "InvalidationReason",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Product");

            migrationBuilder.AlterColumn<Guid>(
                name: "LastPriceChangeRequestId",
                table: "ProductCurrentPrice",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "PriceChangeRequest",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            // Convert numeric status values back to their string representations
            migrationBuilder.Sql(@"
UPDATE ""PriceChangeRequest""
SET ""Status"" = CASE ""Status""
  WHEN '1' THEN 'Pending'
  WHEN '2' THEN 'Applied'
  WHEN '3' THEN 'Expired'
  WHEN '4' THEN 'Cancelled'
  ELSE 'Pending' END;
");

            migrationBuilder.Sql(@"ALTER TABLE ""PriceChangeRequest"" ALTER COLUMN ""Status"" TYPE character varying(20) USING (""Status""::text);");

            migrationBuilder.AlterColumn<string>(
                name: "RejectionReason",
                table: "PriceChangeRequest",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);
        }
    }
}
