using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaPeliculas.Migrations
{
    /// <inheritdoc />
    public partial class PrecioPagadoToReporteCompra : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PrecioPagado",
                table: "ReportesCompra",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrecioPagado",
                table: "ReportesCompra");
        }
    }
}
