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
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Key",
                keyValue: new Guid("7d1456ec-272a-4ad6-9ee7-2281e84d68c0"),
                column: "PasswordHash",
                value: "$2a$11$TKxDQGXY/rzdA0LJBE3.yO6FGHrgwMplsOkiSctXl4nBNaJlS9h6y");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Key",
                keyValue: new Guid("a6b04fe0-e7e1-4385-996f-5525a955734f"),
                column: "PasswordHash",
                value: "$2a$11$PCCNp26qNsiEdRsX1iStfOe5mDjfnK2UL/tFnYT96CIOjMvCBWVbe");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
