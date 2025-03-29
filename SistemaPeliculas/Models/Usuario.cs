using System.ComponentModel.DataAnnotations;

namespace SistemaPeliculas.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "Ingrese un email válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        public string Contrasena { get; set; }

        [Required]
        public int Rol { get; set; } // 1 para Admin, 2 para Cliente

        [Required(ErrorMessage = "El estado civil es obligatorio.")]
        public string EstadoCivil { get; set; }

        [Required(ErrorMessage = "La ciudad es obligatoria.")]
        public string Ciudad { get; set; }

        [Required(ErrorMessage = "El departamento es obligatorio.")]
        public string Departamento { get; set; }

        [Required(ErrorMessage = "La provincia es obligatoria.")]
        public string Provincia { get; set; }

        [Required(ErrorMessage = "El distrito es obligatorio.")]
        public string Distrito { get; set; }

        [Required(ErrorMessage = "El DNI es obligatorio.")]
        [StringLength(8, ErrorMessage = "El DNI debe tener 8 dígitos.")]
        public string DNI { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [Phone(ErrorMessage = "Ingrese un número de teléfono válido.")]
        public string Telefono { get; set; }
    }
}
