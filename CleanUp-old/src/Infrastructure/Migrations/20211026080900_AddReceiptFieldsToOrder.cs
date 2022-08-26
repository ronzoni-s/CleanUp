using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CleanUp.Infrastructure.Migrations
{
    public partial class AddReceiptFieldsToOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "FinalPrice",
                table: "Orders",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Orders",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "ReceiptCashDesk",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReceiptEod",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReceiptErrorDescription",
                table: "Orders",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ReceiptHasPrinted",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ReceiptNumber",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReceiptPrinterNumber",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ReceiptStatus",
                table: "Orders",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReceiptTime",
                table: "Orders",
                type: "datetime2(0)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ReceiptTotal",
                table: "Orders",
                type: "float",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinalPrice",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ReceiptCashDesk",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ReceiptEod",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ReceiptErrorDescription",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ReceiptHasPrinted",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ReceiptNumber",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ReceiptPrinterNumber",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ReceiptStatus",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ReceiptTime",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ReceiptTotal",
                table: "Orders");
        }
    }
}
