using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Skarbnik.Migrations
{
    /// <inheritdoc />
    public partial class start : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Uczniowie",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwisko = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Skrot = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uczniowie", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Wydatki",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Kwota = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wydatki", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UczenWydatki",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UczenId = table.Column<int>(type: "int", nullable: false),
                    WydatekId = table.Column<int>(type: "int", nullable: false),
                    CzyZaplacono = table.Column<bool>(type: "bit", nullable: false),
                    DataZaplaty = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UczenWydatki", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UczenWydatki_Uczniowie_UczenId",
                        column: x => x.UczenId,
                        principalTable: "Uczniowie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UczenWydatki_Wydatki_WydatekId",
                        column: x => x.WydatekId,
                        principalTable: "Wydatki",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UczenWydatki_UczenId_WydatekId",
                table: "UczenWydatki",
                columns: new[] { "UczenId", "WydatekId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UczenWydatki_WydatekId",
                table: "UczenWydatki",
                column: "WydatekId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UczenWydatki");

            migrationBuilder.DropTable(
                name: "Uczniowie");

            migrationBuilder.DropTable(
                name: "Wydatki");
        }
    }
}
