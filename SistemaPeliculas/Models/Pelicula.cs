using System;
using System.ComponentModel.DataAnnotations;

namespace SistemaPeliculas.Models
{
    public class Pelicula
    {
        public int Id { get; set; }
        [Required]
        public string Titulo { get; set; }
        [Required]
        public string DescripcionCorta { get; set; }
        [Required]
        public string DescripcionLarga { get; set; }
        [Required]
        public double Rating { get; set; }
        [Required]
        public string Genero { get; set; }
        [Required]
        public string Actor { get; set; }
        [Required]
        public string Restriccion { get; set; }
        [Required]
        public bool Destacado { get; set; }
        [Required]
        public int Año { get; set; }
        [Required]
        public decimal Precio { get; set; } 
        [Required]
        public string Url { get; set; }
        [Required]
        public string Imagen { get; set; }
        [Required]
        public string Director { get; set; } // Nueva propiedad añadida


        [Required]
        public int Duracion { get; set; } // Nueva propiedad añadida en minutos



        public DateTime FechaEstreno { get; set; }
    }

}

