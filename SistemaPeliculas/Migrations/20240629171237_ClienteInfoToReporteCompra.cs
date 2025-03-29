using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaPeliculas.Migrations
{
    /// <inheritdoc />
    public partial class ClienteInfoToReporteCompra : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DNI",
                table: "ReportesCompra",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Direccion",
                table: "ReportesCompra",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NombreCliente",
                table: "ReportesCompra",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "PrecioUnitario",
                table: "ReportesCompra",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DNI",
                table: "ReportesCompra");

            migrationBuilder.DropColumn(
                name: "Direccion",
                table: "ReportesCompra");

            migrationBuilder.DropColumn(
                name: "NombreCliente",
                table: "ReportesCompra");

            migrationBuilder.DropColumn(
                name: "PrecioUnitario",
                table: "ReportesCompra");
        }
    }
}
