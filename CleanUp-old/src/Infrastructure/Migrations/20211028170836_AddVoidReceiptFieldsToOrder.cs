using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CleanUp.Infrastructure.Migrations
{
    public partial class AddVoidReceiptFieldsToOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceiptHasPrinted",
                table: "Orders");

            migrationBuilder.AddColumn<DateTime>(
                name: "CancellationDateTime",
                table: "Orders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VoidReceiptCashDesk",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VoidReceiptEod",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VoidReceiptErrorDescription",
                table: "Orders",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VoidReceiptNumber",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VoidReceiptPrinterNumber",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "VoidReceiptStatus",
                table: "Orders",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "VoidReceiptTime",
                table: "Orders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "VoidReceiptTotal",
                table: "Orders",
                type: "float",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CancellationDateTime",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "VoidReceiptCashDesk",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "VoidReceiptEod",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "VoidReceiptErrorDescription",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "VoidReceiptNumber",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "VoidReceiptPrinterNumber",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "VoidReceiptStatus",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "VoidReceiptTime",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "VoidReceiptTotal",
                table: "Orders");

            migrationBuilder.AddColumn<bool>(
                name: "ReceiptHasPrinted",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
