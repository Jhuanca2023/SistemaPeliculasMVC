using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaPeliculas.Data;
using SistemaPeliculas.Models;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SistemaPeliculas.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Gestión de Películas

        public IActionResult Peliculas()
        {
            return View(_context.Peliculas.ToList());
        }

        public IActionResult CrearPelicula()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearPelicula([Bind("Titulo,DescripcionCorta,DescripcionLarga,Director,Genero,Actor,Duracion,Restriccion,Rating,Año,Precio,Url,Imagen,Destacado")] Pelicula pelicula)
        {
            if (ModelState.IsValid)
            {
                // Crear o encontrar Actor
                var actor = _context.Actores.SingleOrDefault(a => a.Actor1 == pelicula.Actor);
                if (actor == null)
                {
                    actor = new Actor { Actor1 = pelicula.Actor };
                    _context.Actores.Add(actor);
                }

                // Crear o encontrar Género
                var genero = _context.Generos.SingleOrDefault(g => g.Genero1 == pelicula.Genero);
                if (genero == null)
                {
                    genero = new Genero { Genero1 = pelicula.Genero };
                    _context.Generos.Add(genero);
                }

                // Crear o encontrar Restricción
                var restriccion = _context.Restricciones.SingleOrDefault(r => r.Restriccion1 == pelicula.Restriccion);
                if (restriccion == null)
                {
                    restriccion = new Restriccion { Restriccion1 = pelicula.Restriccion };
                    _context.Restricciones.Add(restriccion);
                }

                _context.Add(pelicula);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Peliculas));
            }
            return View(pelicula);
        }

        public async Task<IActionResult> EditarPelicula(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var pelicula = await _context.Peliculas.FindAsync(id);
            if (pelicula == null)
            {
                return NotFound();
            }
            return View(pelicula);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarPelicula(int id, [Bind("Id,Titulo,DescripcionCorta,DescripcionLarga,Director,Genero,Actor,Duracion,Restriccion,Rating,Año,Precio,Url,Imagen,Destacado")] Pelicula pelicula)
        {
            if (id != pelicula.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Crear o encontrar Actor
                    var actor = _context.Actores.SingleOrDefault(a => a.Actor1 == pelicula.Actor);
                    if (actor == null)
                    {
                        actor = new Actor { Actor1 = pelicula.Actor };
                        _context.Actores.Add(actor);
                    }

                    // Crear o encontrar Género
                    var genero = _context.Generos.SingleOrDefault(g => g.Genero1 == pelicula.Genero);
                    if (genero == null)
                    {
                        genero = new Genero { Genero1 = pelicula.Genero };
                        _context.Generos.Add(genero);
                    }

                    // Crear o encontrar Restricción
                    var restriccion = _context.Restricciones.SingleOrDefault(r => r.Restriccion1 == pelicula.Restriccion);
                    if (restriccion == null)
                    {
                        restriccion = new Restriccion { Restriccion1 = pelicula.Restriccion };
                        _context.Restricciones.Add(restriccion);
                    }

                    _context.Update(pelicula);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PeliculaExists(pelicula.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Peliculas));
            }
            return View(pelicula);
        }
        private bool PeliculaExists(int id)
        {
            return _context.Peliculas.Any(e => e.Id == id);
        }
        public async Task<IActionResult> EliminarPelicula(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var pelicula = await _context.Peliculas.FindAsync(id);
            if (pelicula == null)
            {
                return NotFound();
            }

            return View(pelicula);
        }

        [HttpPost, ActionName("EliminarPeliculaConfirmado")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarPeliculaConfirmado(int id)
        {
            var pelicula = await _context.Peliculas.FindAsync(id);
            if (pelicula == null)
            {
                return NotFound();
            }
            _context.Peliculas.Remove(pelicula);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Peliculas));
        }


        // Gestión de Usuarios

        public IActionResult Usuarios()
        {
            return View(_context.Usuarios.ToList());
        }

        public IActionResult CrearUsuario()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearUsuario(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Usuarios));
            }
            return View(usuario);
        }


        // GET: Usuario/EditarUsuario/5
        // GET: Usuario/EditarUsuario/5
        public IActionResult EditarUsuario(int id)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario == null)
            {
                return NotFound();
            }

            // Crear un SelectList para los roles
            ViewBag.Roles = new SelectList(new[]
            {
        new { Value = 1, Text = "Administrador" },
        new { Value = 2, Text = "Cliente" }
    }, "Value", "Text", usuario.Rol);

            return View(usuario);
        }

        // POST: Usuario/EditarUsuario/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditarUsuario(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                // Comprobar si el modelo recibido contiene los datos correctos
                Console.WriteLine($"Nombre: {usuario.Nombre}, Rol: {usuario.Rol}"); // Debugging

                var usuarioEnDb = _context.Usuarios.Find(usuario.Id);
                if (usuarioEnDb == null)
                {
                    return NotFound();
                }

                // Actualizar los datos del usuario
                usuarioEnDb.Nombre = usuario.Nombre;
                usuarioEnDb.Apellido = usuario.Apellido;
                usuarioEnDb.Email = usuario.Email;
                usuarioEnDb.Contrasena = usuario.Contrasena;
                usuarioEnDb.Rol = usuario.Rol; // Actualizar el rol
                usuarioEnDb.EstadoCivil = usuario.EstadoCivil;
                usuarioEnDb.Ciudad = usuario.Ciudad;
                usuarioEnDb.Departamento = usuario.Departamento;
                usuarioEnDb.Provincia = usuario.Provincia;
                usuarioEnDb.Distrito = usuario.Distrito;
                usuarioEnDb.DNI = usuario.DNI;
                usuarioEnDb.Telefono = usuario.Telefono;

                // Guardar los cambios en la base de datos
                _context.SaveChanges();

                // Redirigir a la página de listado
                return RedirectToAction(nameof(Index));
            }

            // Si hay errores, se vuelve a mostrar el formulario con los datos incorrectos
            return View(usuario);
        }


        // Acción para mostrar la vista de confirmación de eliminación
        public IActionResult EliminarUsuario(int id)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // Acción para confirmar la eliminación
        [HttpPost]
        public IActionResult EliminarUsuarioConfirmado(int id)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            // Eliminar usuario
            _context.Usuarios.Remove(usuario);
            _context.SaveChanges();

            // Redirigir a la vista de gestión de usuarios
            return RedirectToAction("Usuarios");
        }
        public IActionResult Reportes()
        {
            var reportes = _context.ReportesCompra.ToList();
            return View(reportes);
        }
        [HttpPost]
        public IActionResult EliminarReporte(int id)
        {
            var reporte = _context.ReportesCompra.Find(id);
            if (reporte != null)
            {
                _context.ReportesCompra.Remove(reporte);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Reportes));
        }

        // Gestión de otros modelos (Géneros, Actores, etc.) 

        // Gestión de Géneros

        public IActionResult Generos()
        {
            return View(_context.Generos.ToList());
        }

        public IActionResult CrearGenero()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearGenero([Bind("Genero1")] Genero genero)
        {
            if (ModelState.IsValid)
            {
                _context.Add(genero);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Generos));
            }
            return View(genero);
        }

        public async Task<IActionResult> EditarGenero(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var genero = await _context.Generos.FindAsync(id);
            if (genero == null)
            {
                return NotFound();
            }
            return View(genero);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarGenero(int id, [Bind("Id,Descripcion")] Genero genero)
        {
            if (id != genero.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(genero);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Generos));
            }
            return View(genero);
        }

        public async Task<IActionResult> EliminarGenero(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var genero = await _context.Generos.FindAsync(id);
            if (genero == null)
            {
                return NotFound();
            }

            return View(genero);
        }

        [HttpPost, ActionName("EliminarGenero")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarGeneroConfirmado(int id)
        {
            var genero = await _context.Generos.FindAsync(id);
            _context.Generos.Remove(genero);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Generos));
        }

        // Gestión de Actores

        public IActionResult Actores()
        {
            return View(_context.Actores.ToList());
        }

        public IActionResult CrearActor()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearActor([Bind("Actor1")] Actor actor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(actor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Actores));
            }
            return View(actor);
        }

        public async Task<IActionResult> EditarActor(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var actor = await _context.Actores.FindAsync(id);
            if (actor == null)
            {
                return NotFound();
            }
            return View(actor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarActor(int id, [Bind("Id,Descripcion")] Actor actor)
        {
            if (id != actor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(actor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Actores));
            }
            return View(actor);
        }

        public async Task<IActionResult> EliminarActor(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var actor = await _context.Actores.FindAsync(id);
            if (actor == null)
            {
                return NotFound();
            }

            return View(actor);
        }

        [HttpPost, ActionName("EliminarActorConfirmado")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarActorConfirmado(int id)
        {
            var actor = await _context.Actores.FindAsync(id);
            if (actor == null)
            {
                return NotFound();
            }

            _context.Actores.Remove(actor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Actores));
        }




        // Gestión de Restricciones

        public IActionResult Restricciones()
        {
            return View(_context.Restricciones.ToList());
        }

        public IActionResult CrearRestriccion()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearRestriccion([Bind("Restriccion1")] Restriccion restriccion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(restriccion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Restricciones));
            }
            return View(restriccion);
        }
        public async Task<IActionResult> EditarRestriccion(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var restriccion = await _context.Restricciones.FindAsync(id);
            if (restriccion == null)
            {
                return NotFound();
            }
            return View(restriccion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarRestriccion(int id, [Bind("Id,Descripcion")] Restriccion restriccion)
        {
            if (id != restriccion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(restriccion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Restricciones));
            }
            return View(restriccion);
        }

        public async Task<IActionResult> EliminarRestriccion(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var restriccion = await _context.Restricciones.FindAsync(id);
            if (restriccion == null)
            {
                return NotFound();
            }

            return View(restriccion);
        }

        [HttpPost, ActionName("EliminarRestriccion")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarRestriccionConfirmado(int id)
        {
            var restriccion = await _context.Restricciones.FindAsync(id);
            _context.Restricciones.Remove(restriccion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Restricciones));
        }
        public IActionResult Logout()
        {
            
            return RedirectToAction("Login", "Account");
        }





        public IActionResult Reservas()
        {
            var reservas = _context.Reservas.Include(r => r.Pelicula).ToList();
            return View(reservas);
        }



        //Notificacion Correo


        // Acción que recibe el correo y envía la notificación
        [HttpGet]
        public IActionResult EnviarNotificacion(string correo)
        {
            try
            {
                // Crear el mensaje de correo
                var correoMessage = new MailMessage();
                correoMessage.From = new MailAddress("josehuanca612@gmail.com"); // correo de envío
                correoMessage.To.Add(correo); //  correo del cliente
                correoMessage.Subject = "Notificación de Película";
                correoMessage.Body = "¡Gracias por reservar tu película con nosotros!\r\nEstamos encantados de que hayas elegido disfrutar de una película con nosotros. " +
                    "La película ya está disponible y puedes adquirirla a través de nuestra plataforma en cualquier momento.\r\n\r\nDisfruta de tu experiencia de cine desde la comodidad de tu hogar" +
                    ".\r\nSi necesitas realizar algún cambio o tienes alguna pregunta, no dudes en ponerte en contacto con nuestro equipo de soporte. Estamos aquí para ayudarte.\r\n\r\n" +
                    "¡Esperamos que disfrutes mucho de tu película!\r\nSi te gustó, no dudes en dejarnos tus comentarios. Tu opinión es muy importante para seguir mejorando.";
                correoMessage.IsBodyHtml = true; // Si el contenido es HTML (puedes modificarlo si deseas)

                // Configurar el servidor SMTP
                var smtpClient = new SmtpClient("smtp.gmail.com")  //  servidor SMTP
                {
                    Port = 587,
                    Credentials = new NetworkCredential("josehuanca612@gmail.com", "htct frzv uofk zjhk"),
                    EnableSsl = true
                };

                
                smtpClient.Send(correoMessage);

                // Si todo va bien, puedes pasar un mensaje de éxito
                return View(new ErrorViewModel { ErrorMessage = "Notificación enviada correctamente." });
            }
            catch (Exception ex)
            {
                // Si ocurre algún error, pasa el mensaje de error adecuado
                return View(new ErrorViewModel { ErrorMessage = ex.Message });
            }
        }

       
        public IActionResult Confirmacion()
        {
            return View();
        }
    }
}