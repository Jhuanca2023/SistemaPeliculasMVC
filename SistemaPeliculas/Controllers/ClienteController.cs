using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaPeliculas.Data;
using SistemaPeliculas.Models;
using System.Collections.Generic;
using System.Linq;

namespace SistemaPeliculas.Controllers
{
    public class ClienteController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClienteController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            var peliculas = _context.Peliculas.Where(p => p.Destacado).ToList();
            return View(peliculas);
        }

        public IActionResult Peliculas()
        {
            var peliculas = _context.Peliculas.Where(p => p.Destacado).ToList();  // Solo mostrar películas destacadas
            return View(peliculas);
        }


        public IActionResult DetallePelicula(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pelicula = _context.Peliculas.Find(id);
            if (pelicula == null)
            {
                return NotFound();
            }

            return View(pelicula);
        }

        public IActionResult Preventa()
        {
            var peliculas = _context.Peliculas.Where(p => p.Destacado).ToList();
            return View(peliculas);
        }
        public IActionResult Cartelera()
        {
            var peliculas = _context.Peliculas.Where(p => p.Destacado).ToList();  // Solo mostrar películas destacadas
            return View(peliculas);
        }

        public IActionResult Proximamente()
        {
            var proximamentePeliculas = _context.Peliculas.Where(p => !p.Destacado).ToList();  // Mostrar las películas que no están destacadas como próximas
            return View(proximamentePeliculas);
        }

        public IActionResult AgregarAlCarrito(int id)
        {
            var pelicula = _context.Peliculas.Find(id);
            if (pelicula != null)
            {
                List<CarritoItem> carrito;
                if (_httpContextAccessor.HttpContext.Session.Get<List<CarritoItem>>("Carrito") == null)
                {
                    carrito = new List<CarritoItem>();
                }
                else
                {
                    carrito = _httpContextAccessor.HttpContext.Session.Get<List<CarritoItem>>("Carrito");
                }

                var itemEnCarrito = carrito.FirstOrDefault(item => item.Pelicula.Id == id);
                if (itemEnCarrito != null)
                {
                    itemEnCarrito.Cantidad++;
                }
                else
                {
                    carrito.Add(new CarritoItem
                    {
                        Pelicula = pelicula,
                        Cantidad = 1
                    });
                }

                _httpContextAccessor.HttpContext.Session.Set("Carrito", carrito);
            }

            return RedirectToAction(nameof(Carrito));
        }

        public IActionResult Carrito()
        {
            List<CarritoItem> carrito = _httpContextAccessor.HttpContext.Session.Get<List<CarritoItem>>("Carrito");
            return View(carrito);
        }

        public IActionResult EliminarDelCarrito(int id)
        {
            List<CarritoItem> carrito = _httpContextAccessor.HttpContext.Session.Get<List<CarritoItem>>("Carrito");
            var itemEnCarrito = carrito.FirstOrDefault(item => item.Pelicula.Id == id);
            if (itemEnCarrito != null)
            {
                carrito.Remove(itemEnCarrito);
                _httpContextAccessor.HttpContext.Session.Set("Carrito", carrito);
            }

            return RedirectToAction(nameof(Carrito));
        }

        public IActionResult Pagar(string nombreCliente, string dni, string direccion)
        {
            // Obtener el carrito de la sesión
            List<CarritoItem> carrito = _httpContextAccessor.HttpContext.Session.Get<List<CarritoItem>>("Carrito");

            if (carrito != null && carrito.Count > 0)
            {
                foreach (var item in carrito)
                {
                    // Crear un nuevo reporte de compra por cada artículo en el carrito
                    var reporteCompra = new ReporteCompra
                    {
                        PeliculaId = item.Pelicula.Id,
                        PeliculaTitulo = item.Pelicula.Titulo,
                        Cantidad = item.Cantidad, // Asegúrate de que la cantidad actualizada esté reflejada aquí
                        FechaCompra = DateTime.Now,
                        PrecioPagado = item.Pelicula.Precio * item.Cantidad, // Total pagado por este artículo
                        NombreCliente = nombreCliente,
                        DNI = dni,
                        Direccion = direccion
                    };

                    // Agregar el reporte a la base de datos
                    _context.ReportesCompra.Add(reporteCompra);
                }

                // Guardar los cambios en la base de datos
                _context.SaveChanges();

                // Limpiar el carrito después del pago
                _httpContextAccessor.HttpContext.Session.Remove("Carrito");

                // Mensaje de éxito
                TempData["MensajePago"] = "¡Pago exitoso!";
            }

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public IActionResult ActualizarCantidad([FromBody] CarritoItem carritoItem)
        {
            // Obtener el carrito de la sesión
            List<CarritoItem> carrito = _httpContextAccessor.HttpContext.Session.Get<List<CarritoItem>>("Carrito");

            // Buscar el item en el carrito por PeliculaId
            var itemEnCarrito = carrito.FirstOrDefault(item => item.PeliculaId == carritoItem.PeliculaId);

            if (itemEnCarrito != null && carritoItem.Cantidad > 0)
            {
                // Actualizar la cantidad del item
                itemEnCarrito.Cantidad = carritoItem.Cantidad;

                // Guardar el carrito actualizado en la sesión
                _httpContextAccessor.HttpContext.Session.Set("Carrito", carrito);

                // Retornar respuesta de éxito
                return Json(new { success = true, cantidad = itemEnCarrito.Cantidad });
            }

            // Retornar error si algo falla
            return Json(new { success = false, message = "Error al actualizar la cantidad." });
        }



        //cerrar sesion 
        public ActionResult CerrarSesion()
        {

            return RedirectToAction("Login", "Account");
        }
        // Acción para ver el perfil de usuario
        // Acción para visualizar el perfil
        public IActionResult Perfil()
        {
            // Obtener el ID del usuario desde la sesión
            int? usuarioId = HttpContext.Session.GetInt32("UserId");

            // Verificar si el usuario está autenticado
            if (usuarioId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Buscar al usuario por su ID
            var usuario = _context.Usuarios.Find(usuarioId);

            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }


        // Acción para actualizar el perfil (POST)
        [HttpPost]
        public IActionResult ActualizarPerfil(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                int? usuarioId = HttpContext.Session.GetInt32("UserId");

                if (usuarioId == null)
                {
                    TempData["Mensaje"] = "Error: Usuario no autenticado.";
                    return RedirectToAction("Login", "Account");
                }

                var usuarioExistente = _context.Usuarios.Find(usuarioId);

                if (usuarioExistente == null)
                {
                    TempData["Mensaje"] = "Error: Usuario no encontrado.";
                    return RedirectToAction("Perfil");
                }

                // Mantener valores que no se editan
                usuario.Contrasena = usuarioExistente.Contrasena;
                usuario.Rol = usuarioExistente.Rol;

                // Actualizar los valores editables
                usuarioExistente.Nombre = usuario.Nombre;
                usuarioExistente.Apellido = usuario.Apellido;
                usuarioExistente.Email = usuario.Email;
                usuarioExistente.EstadoCivil = usuario.EstadoCivil;
                usuarioExistente.Ciudad = usuario.Ciudad;
                usuarioExistente.Departamento = usuario.Departamento;
                usuarioExistente.Provincia = usuario.Provincia;
                usuarioExistente.Distrito = usuario.Distrito;
                usuarioExistente.DNI = usuario.DNI;
                usuarioExistente.Telefono = usuario.Telefono;

                // Guardar cambios
                _context.SaveChanges();

                TempData["Mensaje"] = "Perfil actualizado exitosamente.";
                return RedirectToAction("Perfil");
            }
            else
            {
                var errores = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                TempData["Mensaje"] = "Errores: " + string.Join(", ", errores);
                return View("Perfil", usuario);
            }
        }



        [HttpPost]
        public IActionResult Reservar(int peliculaId, string correo)
        {
            // Crear un objeto de reserva (modelo)
            var reserva = new Reserva
            {
                PeliculaId = peliculaId,
                CorreoCliente = correo,
                FechaReserva = DateTime.Now
            };

            // Guardar la reserva en la base de datos
            _context.Reservas.Add(reserva);
            _context.SaveChanges();

            // Redirigir a una página de confirmación o mostrar un mensaje de éxito
            TempData["Mensaje"] = "¡Reserva realizada con éxito!";
            return RedirectToAction("Proximamente", "Cliente");
        }

    }
}
