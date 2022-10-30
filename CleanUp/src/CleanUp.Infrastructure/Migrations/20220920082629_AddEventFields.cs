using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanUp.Infrastructure.Migrations
{
    public partial class AddEventFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "dbo",
                table: "Events",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "dbo",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Teacher",
                schema: "dbo",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "dbo",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "dbo",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Teacher",
                schema: "dbo",
                table: "Events");
        }
    }
}
