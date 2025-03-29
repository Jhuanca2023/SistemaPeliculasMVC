namespace SistemaPeliculas.Models
{
    public class CarritoItem
    {
        public int Id { get; set; }
        public int PeliculaId { get; set; }
        public Pelicula Pelicula { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
    }
}
