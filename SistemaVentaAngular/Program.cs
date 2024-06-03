using Microsoft.EntityFrameworkCore;
using SistemaVentaAngular.Models;
using SistemaVentaAngular.Repository.Contratos;
using SistemaVentaAngular.Repository.Implementacion;
using SistemaVentaAngular.Utilidades;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();

// Configura el contexto de la base de datos para usar SQL Server
builder.Services.AddDbContext<DBVentaAngularContext>(options =>
{
    // Especifica la cadena de conexión para la base de datos SQL Server
    options.UseSqlServer(builder.Configuration.GetConnectionString("sqlConection"));
});

// Configura AutoMapper con el perfil definido en AutoMapperProfile
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// Registra las implementaciones de los repositorios en el contenedor de dependencias
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
builder.Services.AddScoped<IRolRepositorio, RolRepositorio>();
builder.Services.AddScoped<ICategoriaRepositorio, CategoriaRepositorio>();
builder.Services.AddScoped<IProductoRepositorio, ProductoRepositorio>();
builder.Services.AddScoped<IVentaRepositorio, VentaRepositorio>();
builder.Services.AddScoped<IDashBoardRepositorio, DashBoardRepositorio>();

var app = builder.Build();

// Configure the HTTP request pipeline.

if (!app.Environment.IsDevelopment())
{
    // Agrega configuraciones específicas para producción
}

// Habilita el uso de archivos estáticos (por ejemplo, HTML, CSS, JavaScript)
app.UseStaticFiles();

// Configura el enrutamiento
app.UseRouting();

// Configura la ruta predeterminada para los controladores MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

// Mapea cualquier solicitud no manejada a "index.html"
app.MapFallbackToFile("index.html");

// Ejecuta la aplicación
app.Run();
