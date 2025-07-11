# SistemaPeliculasMVC

SistemaPeliculasMVC es una aplicación web desarrollada en .NET Core (C#) bajo el patrón Modelo-Vista-Controlador (MVC), orientada a la gestión y preventa de películas. El sistema permite la administración de películas, usuarios, reservas, compras y reportes, integrando funcionalidades tanto para administradores como para clientes.

## Tecnologías Utilizadas

- **Backend**: ASP.NET Core MVC, C#
- **Frontend**: HTML, CSS, JavaScript, Bootstrap
- **Base de datos**: SQL Server, Entity Framework Core
- **Gestión de sesiones:** ASP.NET Core Session
- **Control de versiones:** Git y GitHub

## Funcionalidades Principales

### Para Administradores

- **Gestión de Películas**: Crear, editar, eliminar y destacar películas, asignando información como título, descripción, género, actores, director, duración, restricción de edad, año, precio, URL y portada.
- **Gestión de Usuarios**: Crear, editar y eliminar usuarios.
- **Reportes de Compra**: Visualización y eliminación de reportes de compras realizadas por los clientes.
- **Gestión de Reservas**: Visualización y control de reservas de películas.

### Para Clientes

- **Catálogo de Películas**: Acceso a la cartelera con las películas disponibles, próximas y destacadas.
- **Detalle de Película**: Visualización ampliada de la información de cada película.
- **Preventa y Compra**: Selección de películas, gestión de carrito de compras y generación de tickets/boletas de pago.
- **Reservas**: Reserva de películas próximas a estrenarse.
- **Gestión de Carrito**: Agregar, eliminar y pagar películas en el carrito.

## Estructura del Proyecto

```
SistemaPeliculasMVC/
│
├── SistemaPeliculas/
│   ├── Controllers/
│   │   ├── AdminController.cs       # Lógica administrativa y gestión CRUD
│   │   ├── ClienteController.cs     # Lógica de interacción del cliente (compra, preventa, reservas)
│   │   └── AccountController.cs     # Autenticación y gestión de cuentas
│   ├── Data/
│   │   └── ApplicationDbContext.cs  # Configuración de la base de datos y entidades
│   ├── Models/
│   │   ├── Pelicula.cs              # Modelo de películas
│   │   ├── Reserva.cs               # Modelo de reservas
│   │   └── (otros modelos)          # Usuario, ReporteCompra, CarritoItem, etc.
│   ├── Views/
│   │   ├── Admin/                   # Vistas administrativas
│   │   ├── Cliente/                 # Vistas para clientes (Cartelera, Preventa, Proximamente)
│   │   └── Shared/                  # Layouts generales y componentes compartidos
│   └── Program.cs                   # Configuración principal, servicios y rutinas de arranque
│
└── README.md                        # Documentación del proyecto
```

## Arquitectura y Diseño

- **MVC (Modelo-Vista-Controlador):** Separación clara entre la lógica de negocio (Models), la presentación (Views) y el control de flujo (Controllers).
- **Base de Datos Relacional:** Uso de Entity Framework Core para el mapeo y persistencia de entidades como `Pelicula`, `Reserva`, `Usuario`, y más.
- **Bootstrap y HTML5:** Interfaz moderna y responsiva, con video de fondo en la cartelera y navegación amigable.
- **Gestión de sesiones:** Permite mantener la autenticación y el carrito de compras del usuario.

## Instalación y Ejecución

1. **Clonar el repositorio:**
   ```bash
   git clone https://github.com/Jhuanca2023/SistemaPeliculasMVC.git
   ```
2. **Configurar la base de datos:**
   - Modifica el archivo `appsettings.json` para incluir tu cadena de conexión SQL Server.
3. **Restaurar dependencias y migrar la base de datos:**
   ```bash
   dotnet restore
   dotnet ef database update
   ```
4. **Ejecutar la aplicación:**
   ```bash
   dotnet run
   ```
5. **Acceder desde tu navegador:**
   - Por defecto: `https://localhost:5001/`

## Usuario Administrador por Defecto

El sistema inicializa un usuario administrador por defecto para acceder al panel de gestión. Este usuario se configura en el archivo de migración, archivo de configuración (por ejemplo, `SeedData` en el arranque del proyecto), o en un archivo especial de configuración.

**Ejemplo de usuario administrador creado en el seed de datos:**

```csharp
// En SeedData.cs o migración inicial
var adminUser = new Usuario
{
    Nombre = "Admin",
    Apellido = "Principal",
    Email = "admin@example.com",
    Contrasena = "admin123", // Cambia la contraseña en producción
    Rol = 1 // 1 = Administrador
};
// Solo se crea si no existe
if (!context.Usuarios.Any(u => u.Email == adminUser.Email))
{
    context.Usuarios.Add(adminUser);
    context.SaveChanges();
}
```

> **Importante:** Cambia la contraseña del usuario administrador en producción y/o elimina este usuario si creas tu propio flujo de registro de administradores.

## Uso Básico

- **Administrador:** Iniciar sesión con el usuario administrador por defecto y acceder al panel para gestionar películas, usuarios, reservas y reportes.
- **Cliente:** Explorar la cartelera, realizar compras y reservas, gestionar el carrito y ver detalles de películas.
- **Compra de Película:** Navegar a "Preventa", seleccionar película, realizar compra y generar ticket/boleta.
- **Reserva de Película:** En la sección "Próximamente", reservar y recibir confirmación.

## Ejemplo de Modelo de Película

```csharp
public class Pelicula
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public string DescripcionCorta { get; set; }
    public string DescripcionLarga { get; set; }
    public string Genero { get; set; }
    public string Actor { get; set; }
    public string Director { get; set; }
    public int Duracion { get; set; }
    public string Restriccion { get; set; }
    public double Rating { get; set; }
    public int Año { get; set; }
    public decimal Precio { get; set; }
    public string Url { get; set; }
    public string Imagen { get; set; }
    public bool Destacado { get; set; }
    public DateTime FechaEstreno { get; set; }
}
```

## Contribuciones

Las contribuciones son bienvenidas. Puedes crear issues para reportar bugs o sugerir mejoras, y enviar pull requests para contribuir al desarrollo.

## Autor

Desarrollado por [Jhuanca2023](https://github.com/Jhuanca2023). Especializado en soluciones ágiles y modernas para la gestión de películas, combinando backend y frontend para una experiencia integral.

---

¡Optimiza la gestión y preventa de películas con SistemaPeliculasMVC!
