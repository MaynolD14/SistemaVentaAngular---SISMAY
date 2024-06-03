using Microsoft.EntityFrameworkCore;
using SistemaVentaAngular.Models;
using SistemaVentaAngular.Repository.Contratos;
using System.Globalization;

namespace SistemaVentaAngular.Repository.Implementacion
{
    // Implementación del repositorio de ventas que sigue el contrato IVentaRepositorio
    public class VentaRepositorio : IVentaRepositorio
    {
        // Contexto de la base de datos inyectado
        private readonly DBVentaAngularContext _dbcontext;

        // Constructor que inicializa el contexto de la base de datos mediante inyección de dependencias
        public VentaRepositorio(DBVentaAngularContext context)
        {
            _dbcontext = context;
        }

        // Método para registrar una nueva venta
        public async Task<Venta> Registrar(Venta entidad)
        {
            Venta VentaGenerada = new Venta();

            // Usaremos una transacción para asegurar la consistencia de los datos en caso de error
            using (var transaction = _dbcontext.Database.BeginTransaction())
            {
                int CantidadDigitos = 4;
                try
                {
                    // Actualiza el stock de cada producto en la venta
                    foreach (DetalleVenta dv in entidad.DetalleVenta)
                    {
                        Producto producto_encontrado = _dbcontext.Productos.Where(p => p.IdProducto == dv.IdProducto).First();
                        producto_encontrado.Stock -= dv.Cantidad;
                        _dbcontext.Productos.Update(producto_encontrado);
                    }
                    await _dbcontext.SaveChangesAsync();

                    // Actualiza el número de documento
                    NumeroDocumento correlativo = _dbcontext.NumeroDocumentos.First();
                    correlativo.UltimoNumero += 1;
                    correlativo.FechaRegistro = DateTime.Now;
                    _dbcontext.NumeroDocumentos.Update(correlativo);
                    await _dbcontext.SaveChangesAsync();

                    // Genera el número de venta con ceros a la izquierda
                    string ceros = string.Concat(Enumerable.Repeat("0", CantidadDigitos));
                    string numeroVenta = ceros + correlativo.UltimoNumero.ToString();
                    numeroVenta = numeroVenta.Substring(numeroVenta.Length - CantidadDigitos, CantidadDigitos);

                    // Asigna el número de documento generado a la venta
                    entidad.NumeroDocumento = numeroVenta;

                    // Añade la venta al contexto y guarda los cambios
                    await _dbcontext.Venta.AddAsync(entidad);
                    await _dbcontext.SaveChangesAsync();

                    VentaGenerada = entidad;

                    // Confirma la transacción
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    // Revertir la transacción en caso de error
                    transaction.Rollback();
                    throw;
                }
            }

            return VentaGenerada;
        }

        // Método para obtener el historial de ventas filtrado por fecha o número de venta
        public async Task<List<Venta>> Historial(string buscarPor, string numeroVenta, string fechaInicio, string fechaFin)
        {
            IQueryable<Venta> query = _dbcontext.Venta;

            if (buscarPor == "fecha")
            {
                DateTime fech_Inicio = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", new CultureInfo("es-PE"));
                DateTime fech_Fin = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", new CultureInfo("es-PE"));

                return await query.Where(v =>
                    v.FechaRegistro.Value.Date >= fech_Inicio.Date &&
                    v.FechaRegistro.Value.Date <= fech_Fin.Date
                )
                .Include(dv => dv.DetalleVenta)
                .ThenInclude(p => p.IdProductoNavigation)
                .ToListAsync();
            }
            else
            {
                return await query.Where(v => v.NumeroDocumento == numeroVenta)
                  .Include(dv => dv.DetalleVenta)
                  .ThenInclude(p => p.IdProductoNavigation)
                  .ToListAsync();
            }
        }

        // Método para generar un reporte de ventas en un rango de fechas
        public async Task<List<DetalleVenta>> Reporte(DateTime FechaInicio, DateTime FechaFin)
        {
            List<DetalleVenta> listaResumen = await _dbcontext.DetalleVenta
                .Include(p => p.IdProductoNavigation)
                .Include(v => v.IdVentaNavigation)
                .Where(dv => dv.IdVentaNavigation.FechaRegistro.Value.Date >= FechaInicio.Date && dv.IdVentaNavigation.FechaRegistro.Value.Date <= FechaFin.Date)
                .ToListAsync();

            return listaResumen;
        }
    }
}
