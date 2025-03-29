namespace SistemaPeliculas.Models
{
    public class ReporteCompra
    {
        public int Id { get; set; }
        public int PeliculaId { get; set; }
        public string PeliculaTitulo { get; set; }
        public int Cantidad { get; set; }
        public DateTime FechaCompra { get; set; }
        public decimal PrecioPagado { get; set; }

        // Campos para la información del cliente
        public string NombreCliente { get; set; }
        public string DNI { get; set; }
        public string Direccion { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Total { get { return Cantidad * PrecioUnitario; } }

        
    }
}
