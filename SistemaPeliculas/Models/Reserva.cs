namespace SistemaPeliculas.Models
{
    public class Reserva
    {
        public int Id { get; set; }
        public int PeliculaId { get; set; }
        public string CorreoCliente { get; set; }
        public DateTime FechaReserva { get; set; }

        // Relación con la película (si aplica)
        public Pelicula Pelicula { get; set; }
    }

}
