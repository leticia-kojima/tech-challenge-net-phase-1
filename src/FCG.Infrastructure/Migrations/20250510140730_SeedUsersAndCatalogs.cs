using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FCG.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedUsersAndCatalogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Catalogs",
                columns: new[] { "Key", "DeletedAt", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("1cb7a523-7866-47c8-90a5-bdec86cc02a0"), null, "Content related to the REST and RESTful...", "REST and RESTful" },
                    { new Guid("69c72c65-d036-4fa4-8f84-7cd37449a009"), null, "Cloud solutions with AWS...", "AWS Cloud" },
                    { new Guid("82b63727-03b4-48db-9e5d-cde73ee9c68a"), null, "Cloud solutions with Azure...", "Azure Cloud" },
                    { new Guid("d5460750-bb4d-40e8-9de5-c28563043e36"), null, "C#, Java...", "Programming Languages" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Key", "DeletedAt", "Email", "FullName", "PasswordHash", "Role" },
                values: new object[,]
                {
                    { new Guid("7d1456ec-272a-4ad6-9ee7-2281e84d68c0"), null, "user@fcg.test.com.br", "Usuário", "7d6721d6-6cb4-4ade-aab2-38a549964b09", 2 },
                    { new Guid("a6b04fe0-e7e1-4385-996f-5525a955734f"), null, "admin@fcg.test.com.br", "Administrador", "7d6721d6-6cb4-4ade-aab2-38a549964b09", 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Catalogs",
                keyColumn: "Key",
                keyValue: new Guid("1cb7a523-7866-47c8-90a5-bdec86cc02a0"));

            migrationBuilder.DeleteData(
                table: "Catalogs",
                keyColumn: "Key",
                keyValue: new Guid("69c72c65-d036-4fa4-8f84-7cd37449a009"));

            migrationBuilder.DeleteData(
                table: "Catalogs",
                keyColumn: "Key",
                keyValue: new Guid("82b63727-03b4-48db-9e5d-cde73ee9c68a"));

            migrationBuilder.DeleteData(
                table: "Catalogs",
                keyColumn: "Key",
                keyValue: new Guid("d5460750-bb4d-40e8-9de5-c28563043e36"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Key",
                keyValue: new Guid("7d1456ec-272a-4ad6-9ee7-2281e84d68c0"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Key",
                keyValue: new Guid("a6b04fe0-e7e1-4385-996f-5525a955734f"));
        }
    }
}
