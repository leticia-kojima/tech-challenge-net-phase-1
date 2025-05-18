using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FCG.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserPasswordHash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameEvaluation_Games_GameKey1",
                table: "GameEvaluation");

            migrationBuilder.DropIndex(
                name: "IX_GameEvaluation_GameKey1",
                table: "GameEvaluation");

            migrationBuilder.DropColumn(
                name: "GameKey1",
                table: "GameEvaluation");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Key",
                keyValue: new Guid("7d1456ec-272a-4ad6-9ee7-2281e84d68c0"),
                column: "PasswordHash",
                value: "$2a$11$1RZ55jTgKvXaaK2jN4qmF.x1DNI2vJqS27.ePmYE1smPSTyB7AXDO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Key",
                keyValue: new Guid("a6b04fe0-e7e1-4385-996f-5525a955734f"),
                column: "PasswordHash",
                value: "$2a$11$bmADClM6Rg/A51PbN4YZA.8iMU2p9mPakBp1TaJB8FtMZS22AFqHG");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GameKey1",
                table: "GameEvaluation",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Key",
                keyValue: new Guid("7d1456ec-272a-4ad6-9ee7-2281e84d68c0"),
                column: "PasswordHash",
                value: "7d6721d6-6cb4-4ade-aab2-38a549964b09");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Key",
                keyValue: new Guid("a6b04fe0-e7e1-4385-996f-5525a955734f"),
                column: "PasswordHash",
                value: "7d6721d6-6cb4-4ade-aab2-38a549964b09");

            migrationBuilder.CreateIndex(
                name: "IX_GameEvaluation_GameKey1",
                table: "GameEvaluation",
                column: "GameKey1");

            migrationBuilder.AddForeignKey(
                name: "FK_GameEvaluation_Games_GameKey1",
                table: "GameEvaluation",
                column: "GameKey1",
                principalTable: "Games",
                principalColumn: "Key");
        }
    }
}
