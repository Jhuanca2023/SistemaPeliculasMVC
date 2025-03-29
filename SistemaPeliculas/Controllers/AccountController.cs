using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using SistemaPeliculas.Data;
using SistemaPeliculas.Models;
using System.Linq;

namespace SistemaPeliculas.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Usuarios.SingleOrDefault(u => u.Email == email && u.Contrasena == password);
            if (user != null)
            {
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetInt32("UserRole", user.Rol);
                if (user.Rol == 1)
                {
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    return RedirectToAction("Index", "Cliente");
                }
            }
            ViewBag.Error = "Credenciales inválidas";
            return View();
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                // Verificar si el email ya está registrado
                if (_context.Usuarios.Any(u => u.Email == usuario.Email))
                {
                    ViewBag.Error = "El correo electrónico ya está registrado.";
                    return View();
                }

                // Asignar el rol por defecto (cliente)
                usuario.Rol = 2;

                // Guardar el nuevo usuario en la base de datos
                _context.Usuarios.Add(usuario);
                _context.SaveChanges();

                // Almacenar el mensaje de éxito en TempData
                TempData["SuccessMessage"] = "Registro exitoso. Por favor, inicia sesión.";

                return RedirectToAction("Login");
            }

            return View(usuario);
        }

        // GET: /Account/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
