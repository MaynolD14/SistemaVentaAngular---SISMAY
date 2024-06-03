using SistemaVentaAngular.Models;
using SistemaVentaAngular.Repository.Contratos;
using System.Globalization;

namespace SistemaVentaAngular.Repository.Implementacion
{
    // Implementación del repositorio del dashboard que sigue el contrato IDashBoardRepositorio
    public class DashBoardRepositorio : IDashBoardRepositorio
    {
        // Contexto de la base de datos inyectado
        private readonly DBVentaAngularContext _dbcontext;

        // Constructor que inicializa el contexto de la base de datos mediante inyección de dependencias
        public DashBoardRepositorio(DBVentaAngularContext context)
        {
            _dbcontext = context;
        }

        // Método para obtener el total de ventas de la última semana
        public async Task<int> TotalVentasUltimaSemana()
        {
            int total = 0;
            try
            {
                // Consulta para obtener todas las ventas como IQueryable
                IQueryable<Venta> _ventaQuery = _dbcontext.Venta.AsQueryable();

                // Verifica si hay ventas
                if (_ventaQuery.Count() > 0)
                {
                    // Obtiene la fecha de la última venta registrada
                    DateTime? ultimaFecha = _dbcontext.Venta.OrderByDescending(v => v.FechaRegistro).Select(v => v.FechaRegistro).First();
                    // Retrocede 7 días desde la última fecha registrada
                    ultimaFecha = ultimaFecha.Value.AddDays(-7);

                    // Filtra las ventas de los últimos 7 días
                    IQueryable<Venta> query = _dbcontext.Venta.Where(v => v.FechaRegistro.Value.Date >= ultimaFecha.Value.Date);
                    // Cuenta el total de ventas en los últimos 7 días
                    total = query.Count();
                }
                return total;
            }
            catch
            {
                // Si ocurre una excepción, se lanza para ser manejada por el llamador
                throw;
            }
        }

        // Método para obtener el total de ingresos de la última semana
        public async Task<string> TotalIngresosUltimaSemana()
        {
            decimal resultado = 0;
            try
            {
                // Consulta para obtener todas las ventas como IQueryable
                IQueryable<Venta> _ventaQuery = _dbcontext.Venta.AsQueryable();

                // Verifica si hay ventas
                if (_ventaQuery.Count() > 0)
                {
                    // Obtiene la fecha de la última venta registrada
                    DateTime? ultimaFecha = _dbcontext.Venta.OrderByDescending(v => v.FechaRegistro).Select(v => v.FechaRegistro).First();
                    // Retrocede 7 días desde la última fecha registrada
                    ultimaFecha = ultimaFecha.Value.AddDays(-7);

                    // Filtra las ventas de los últimos 7 días
                    IQueryable<Venta> query = _dbcontext.Venta.Where(v => v.FechaRegistro.Value.Date >= ultimaFecha.Value.Date);

                    // Suma los totales de las ventas en los últimos 7 días
                    resultado = query
                        .Select(v => v.Total)
                        .Sum(v => v.Value);
                }

                // Convierte el resultado a cadena con formato de moneda en español (Perú) (El del salvador no me funcionó XD, inche shit) 
                return Convert.ToString(resultado, new CultureInfo("es-PE"));
            }
            catch
            {
                // Si ocurre una excepción, se lanza para ser manejada por el llamador
                throw;
            }
        }

        // Método para obtener el total de productos
        public async Task<int> TotalProductos()
        {
            try
            {
                // Consulta para obtener todos los productos
                IQueryable<Producto> query = _dbcontext.Productos;
                // Cuenta el total de productos
                int total = query.Count();
                return total;
            }
            catch
            {
                // Si ocurre una excepción, se lanza para ser manejada por el llamador
                throw;
            }
        }

        // Método para obtener el total de ventas diarias de la última semana
        public async Task<Dictionary<string, int>> VentasUltimaSemana()
        {
            Dictionary<string, int> resultado = new Dictionary<string, int>();
            try
            {
                // Consulta para obtener todas las ventas como IQueryable
                IQueryable<Venta> _ventaQuery = _dbcontext.Venta.AsQueryable();
                // Verifica si hay ventas
                if (_ventaQuery.Count() > 0)
                {
                    // Obtiene la fecha de la última venta registrada
                    DateTime? ultimaFecha = _dbcontext.Venta.OrderByDescending(v => v.FechaRegistro).Select(v => v.FechaRegistro).First();
                    // Retrocede 7 días desde la última fecha registrada
                    ultimaFecha = ultimaFecha.Value.AddDays(-7);

                    // Filtra las ventas de los últimos 7 días
                    IQueryable<Venta> query = _dbcontext.Venta.Where(v => v.FechaRegistro.Value.Date >= ultimaFecha.Value.Date);

                    // Agrupa las ventas por fecha y cuenta el total por día
                    resultado = query
                        .GroupBy(v => v.FechaRegistro.Value.Date).OrderBy(g => g.Key)
                        .Select(dv => new { fecha = dv.Key.ToString("dd/MM/yyyy"), total = dv.Count() })
                        .ToDictionary(keySelector: r => r.fecha, elementSelector: r => r.total);
                }

                return resultado;
            }
            catch
            {
                // Si ocurre una excepción, se lanza para ser manejada por el llamador
                throw;
            }
        }
    }
}
