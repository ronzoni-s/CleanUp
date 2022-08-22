using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ErbertPranzi.Infrastructure.Migrations
{
    public partial class UpdatedFieldInAuditable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastModifiedOn",
                schema: "Identity",
                table: "Users",
                newName: "LastUpdated");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                schema: "Identity",
                table: "Users",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "LastModifiedOn",
                schema: "Identity",
                table: "Roles",
                newName: "LastUpdated");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                schema: "Identity",
                table: "Roles",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "LastModifiedOn",
                schema: "Identity",
                table: "RoleClaims",
                newName: "LastUpdated");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                schema: "Identity",
                table: "RoleClaims",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "LastModifiedOn",
                table: "Products",
                newName: "LastUpdated");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "Products",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "LastModifiedOn",
                table: "DocumentTypes",
                newName: "LastUpdated");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "DocumentTypes",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "LastModifiedOn",
                table: "Documents",
                newName: "LastUpdated");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "Documents",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "LastModifiedOn",
                table: "DocumentExtendedAttributes",
                newName: "LastUpdated");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "DocumentExtendedAttributes",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "LastModifiedOn",
                table: "Brands",
                newName: "LastUpdated");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "Brands",
                newName: "Created");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                schema: "Identity",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastUpdatedBy",
                schema: "Identity",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                schema: "Identity",
                table: "Roles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastUpdatedBy",
                schema: "Identity",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                schema: "Identity",
                table: "RoleClaims",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastUpdatedBy",
                schema: "Identity",
                table: "RoleClaims",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "Products",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastUpdatedBy",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "DocumentTypes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastUpdatedBy",
                table: "DocumentTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "Documents",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastUpdatedBy",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "DocumentExtendedAttributes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastUpdatedBy",
                table: "DocumentExtendedAttributes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "Brands",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastUpdatedBy",
                table: "Brands",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastModified",
                schema: "Identity",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastUpdatedBy",
                schema: "Identity",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastModified",
                schema: "Identity",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "LastUpdatedBy",
                schema: "Identity",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "LastModified",
                schema: "Identity",
                table: "RoleClaims");

            migrationBuilder.DropColumn(
                name: "LastUpdatedBy",
                schema: "Identity",
                table: "RoleClaims");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "LastUpdatedBy",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "DocumentTypes");

            migrationBuilder.DropColumn(
                name: "LastUpdatedBy",
                table: "DocumentTypes");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "LastUpdatedBy",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "DocumentExtendedAttributes");

            migrationBuilder.DropColumn(
                name: "LastUpdatedBy",
                table: "DocumentExtendedAttributes");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "LastUpdatedBy",
                table: "Brands");

            migrationBuilder.RenameColumn(
                name: "LastUpdated",
                schema: "Identity",
                table: "Users",
                newName: "LastModifiedOn");

            migrationBuilder.RenameColumn(
                name: "Created",
                schema: "Identity",
                table: "Users",
                newName: "CreatedOn");

            migrationBuilder.RenameColumn(
                name: "LastUpdated",
                schema: "Identity",
                table: "Roles",
                newName: "LastModifiedOn");

            migrationBuilder.RenameColumn(
                name: "Created",
                schema: "Identity",
                table: "Roles",
                newName: "CreatedOn");

            migrationBuilder.RenameColumn(
                name: "LastUpdated",
                schema: "Identity",
                table: "RoleClaims",
                newName: "LastModifiedOn");

            migrationBuilder.RenameColumn(
                name: "Created",
                schema: "Identity",
                table: "RoleClaims",
                newName: "CreatedOn");

            migrationBuilder.RenameColumn(
                name: "LastUpdated",
                table: "Products",
                newName: "LastModifiedOn");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "Products",
                newName: "CreatedOn");

            migrationBuilder.RenameColumn(
                name: "LastUpdated",
                table: "DocumentTypes",
                newName: "LastModifiedOn");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "DocumentTypes",
                newName: "CreatedOn");

            migrationBuilder.RenameColumn(
                name: "LastUpdated",
                table: "Documents",
                newName: "LastModifiedOn");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "Documents",
                newName: "CreatedOn");

            migrationBuilder.RenameColumn(
                name: "LastUpdated",
                table: "DocumentExtendedAttributes",
                newName: "LastModifiedOn");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "DocumentExtendedAttributes",
                newName: "CreatedOn");

            migrationBuilder.RenameColumn(
                name: "LastUpdated",
                table: "Brands",
                newName: "LastModifiedOn");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "Brands",
                newName: "CreatedOn");
        }
    }
}
