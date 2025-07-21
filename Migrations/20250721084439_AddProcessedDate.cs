using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace week2_Task.Migrations
{
    /// <inheritdoc />
    public partial class AddProcessedDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Merchants_MerchantId1",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_MerchantId1",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "MerchantId1",
                table: "Payments");

            migrationBuilder.AlterColumn<string>(
                name: "Currency",
                table: "Payments",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "ProcessedDate",
                table: "Payments",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProcessedDate",
                table: "Payments");

            migrationBuilder.AlterColumn<string>(
                name: "Currency",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(3)",
                oldMaxLength: 3);

            migrationBuilder.AddColumn<Guid>(
                name: "MerchantId1",
                table: "Payments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_MerchantId1",
                table: "Payments",
                column: "MerchantId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Merchants_MerchantId1",
                table: "Payments",
                column: "MerchantId1",
                principalTable: "Merchants",
                principalColumn: "Id");
        }
    }
}
