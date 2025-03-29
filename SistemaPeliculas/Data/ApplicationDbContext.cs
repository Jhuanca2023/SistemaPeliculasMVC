using Microsoft.EntityFrameworkCore;
using SistemaPeliculas.Models;
using System.Collections.Generic;

namespace SistemaPeliculas.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Pelicula> Peliculas { get; set; }
        public DbSet<ReporteCompra> ReportesCompra { get; set; }
        public DbSet<Genero> Generos { get; set; }
        public DbSet<Actor> Actores { get; set; }
        public DbSet<Restriccion> Restricciones { get; set; }

        public DbSet<CarritoItem> CarritoItem { get; set; }
        public DbSet<Reserva> Reservas { get; set; }


    }
}
