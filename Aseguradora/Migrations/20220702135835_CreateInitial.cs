using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aseguradora.Migrations
{
    public partial class CreateInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    CedulaCliente = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NombreCliente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Edad = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.CedulaCliente);
                });

            migrationBuilder.CreateTable(
                name: "Seguros",
                columns: table => new
                {
                    CodigoSeguro = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SumaAsegurada = table.Column<float>(type: "real", nullable: false),
                    PrimaSeguro = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seguros", x => x.CodigoSeguro);
                });

            migrationBuilder.CreateTable(
                name: "ClienteSeguro",
                columns: table => new
                {
                    CodigoSeguro = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CedulaCliente = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClienteSeguro", x => new { x.CodigoSeguro, x.CedulaCliente });
                    table.ForeignKey(
                        name: "FK_ClienteSeguro_Clientes_CedulaCliente",
                        column: x => x.CedulaCliente,
                        principalTable: "Clientes",
                        principalColumn: "CedulaCliente",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClienteSeguro_Seguros_CodigoSeguro",
                        column: x => x.CodigoSeguro,
                        principalTable: "Seguros",
                        principalColumn: "CodigoSeguro",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClienteSeguro_CedulaCliente",
                table: "ClienteSeguro",
                column: "CedulaCliente");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClienteSeguro");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Seguros");
        }
    }
}
