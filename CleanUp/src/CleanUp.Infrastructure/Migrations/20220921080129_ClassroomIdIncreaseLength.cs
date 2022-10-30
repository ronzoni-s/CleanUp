using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanUp.Infrastructure.Migrations
{
    public partial class ClassroomIdIncreaseLength : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"ALTER TABLE [dbo].[Events]
                DROP CONSTRAINT [FK_Events_Classrooms_ClassroomId];");

            migrationBuilder.AlterColumn<string>(
                name: "ClassroomId",
                schema: "dbo",
                table: "Events",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(4)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                schema: "dbo",
                table: "Events",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                schema: "dbo",
                table: "Classrooms",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(4)",
                oldMaxLength: 4);

            migrationBuilder.Sql(@"ALTER TABLE [dbo].[Events]
                ADD CONSTRAINT [FK_Events_Classrooms_ClassroomId] FOREIGN KEY ([ClassroomId]) REFERENCES [dbo].[Classrooms] ([Id]);");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                schema: "dbo",
                table: "Events");

            migrationBuilder.AlterColumn<string>(
                name: "ClassroomId",
                schema: "dbo",
                table: "Events",
                type: "nvarchar(4)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                schema: "dbo",
                table: "Classrooms",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);
        }
    }
}
