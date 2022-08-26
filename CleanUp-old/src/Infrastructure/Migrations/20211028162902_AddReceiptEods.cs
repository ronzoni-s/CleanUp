using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CleanUp.Infrastructure.Migrations
{
    public partial class AddReceiptEods : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReceiptEods",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nchar(36)", fixedLength: true, maxLength: 36, nullable: false),
                    ReceiptPrinterNumber = table.Column<int>(type: "int", nullable: false),
                    ReceiptCashDesk = table.Column<int>(type: "int", nullable: false),
                    ReceiptStatus = table.Column<bool>(type: "bit", nullable: false),
                    ReceiptTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReceiptNumber = table.Column<int>(type: "int", nullable: false),
                    FinancialAmount = table.Column<double>(type: "float", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiptEods", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReceiptEods");
        }
    }
}
