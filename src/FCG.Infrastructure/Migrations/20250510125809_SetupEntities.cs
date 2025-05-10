using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FCG.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SetupEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Catalogs",
                columns: table => new
                {
                    Key = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    DeletedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Catalogs", x => x.Key);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Key = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    FullName = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Role = table.Column<int>(type: "int", nullable: false),
                    PasswordHash = table.Column<string>(type: "varchar(450)", maxLength: 450, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    DeletedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Key);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Key = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Title = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CatalogKey = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    DeletedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Key);
                    table.ForeignKey(
                        name: "FK_Games_Catalogs_CatalogKey",
                        column: x => x.CatalogKey,
                        principalTable: "Catalogs",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "GameDownload",
                columns: table => new
                {
                    UserKey = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    GameKey = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Key = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    DeletedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameDownload", x => new { x.UserKey, x.GameKey });
                    table.ForeignKey(
                        name: "FK_GameDownload_Games_GameKey",
                        column: x => x.GameKey,
                        principalTable: "Games",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameDownload_Users_UserKey",
                        column: x => x.UserKey,
                        principalTable: "Users",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "GameEvaluation",
                columns: table => new
                {
                    UserKey = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    GameKey = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Stars = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    GameKey1 = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Key = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    DeletedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameEvaluation", x => new { x.UserKey, x.GameKey });
                    table.ForeignKey(
                        name: "FK_GameEvaluation_Games_GameKey",
                        column: x => x.GameKey,
                        principalTable: "Games",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameEvaluation_Games_GameKey1",
                        column: x => x.GameKey1,
                        principalTable: "Games",
                        principalColumn: "Key");
                    table.ForeignKey(
                        name: "FK_GameEvaluation_Users_UserKey",
                        column: x => x.UserKey,
                        principalTable: "Users",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Catalogs_Name",
                table: "Catalogs",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameDownload_GameKey",
                table: "GameDownload",
                column: "GameKey");

            migrationBuilder.CreateIndex(
                name: "IX_GameEvaluation_GameKey",
                table: "GameEvaluation",
                column: "GameKey");

            migrationBuilder.CreateIndex(
                name: "IX_GameEvaluation_GameKey1",
                table: "GameEvaluation",
                column: "GameKey1");

            migrationBuilder.CreateIndex(
                name: "IX_Games_CatalogKey",
                table: "Games",
                column: "CatalogKey");

            migrationBuilder.CreateIndex(
                name: "IX_Games_Title",
                table: "Games",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameDownload");

            migrationBuilder.DropTable(
                name: "GameEvaluation");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Catalogs");
        }
    }
}
