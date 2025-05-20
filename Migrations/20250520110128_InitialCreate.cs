using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApexVolley.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Match",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Avversari = table.Column<string>(type: "nvarchar(120)", nullable: false),
                    Luogo = table.Column<string>(type: "nvarchar(120)", nullable: false),
                    Risultato = table.Column<string>(type: "nvarchar(15)", nullable: false),
                    RisultatoSet1 = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    RisultatoSet2 = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    RisultatoSet3 = table.Column<string>(type: "nvarchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Match", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Player",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(70)", nullable: true),
                    Cognome = table.Column<string>(type: "nvarchar(80)", nullable: true),
                    DataNascita = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ruolo = table.Column<string>(type: "nvarchar(45)", nullable: true),
                    AltezzaCm = table.Column<int>(type: "int", nullable: false),
                    NumeroMaglia = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Player", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Match");

            migrationBuilder.DropTable(
                name: "Player");
        }
    }
}
