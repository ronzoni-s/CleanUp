using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanUp.Infrastructure.Migrations
{
    public partial class TenantTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE SCHEMA multitenancy");
            
            migrationBuilder.Sql(@"CREATE TABLE [multitenancy].[Tenants] (
	            [Id] [nvarchar](64) NOT NULL,
	            [Identifier] [nvarchar](450) NULL,
	            [Name] [nvarchar](max) NULL,
	            [ConnectionString] [nvarchar](max) NULL,
	            [AdminEmail] [nvarchar](max) NULL,
	            [IsActive] [bit] NOT NULL,
	            [ValidUpto] [datetime2](7) NOT NULL,
	            [Issuer] [nvarchar](max) NULL,
	            [OpenIdConnectAuthority] [nvarchar](max) NULL,
	            [OpenIdConnectClientId] [nvarchar](max) NULL,
	            [OpenIdConnectClientSecret] [nvarchar](max) NULL,
                 CONSTRAINT [PK_Tenants] PRIMARY KEY CLUSTERED 
                (
	                [Id] ASC
                ))");
			
			migrationBuilder.Sql(@"INSERT INTO [multitenancy].[Tenants]
                       ([Id]
                       ,[Identifier]
                       ,[Name]
                       ,[ConnectionString]
                       ,[AdminEmail]
                       ,[IsActive]
                       ,[ValidUpto])
                    VALUES
                       ('b5c6617e-e0d8-4739-bcd0-1260cdfe75d0'
                       ,'root'
                       ,'CleanUp'
                       ,'Server=(localdb)\\mssqllocaldb;Database=CleanUp;Trusted_Connection=True;MultipleActiveResultSets=true'
                       ,'sronzoni99@gmail.com'
                       ,1
                       ,'2999-12-31 00:00:00.0000000');");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql(@"DROP TABLE [multitenancy].[Tenants]");
			migrationBuilder.Sql(@"DROP SCHEMA multitenancy");
        }
    }
}
