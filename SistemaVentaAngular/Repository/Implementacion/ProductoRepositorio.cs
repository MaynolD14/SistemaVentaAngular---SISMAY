using Microsoft.EntityFrameworkCore;
using SistemaVentaAngular.Models;
using SistemaVentaAngular.Repository.Contratos;
using System.Linq.Expressions;

namespace SistemaVentaAngular.Repository.Implementacion
{
    // Implementación del repositorio de productos que sigue el contrato IProductoRepositorio
    public class ProductoRepositorio : IProductoRepositorio
    {
        // Contexto de la base de datos inyectado
        private readonly DBVentaAngularContext _dbContext;

        // Constructor que inicializa el contexto de la base de datos mediante inyección de dependencias
        public ProductoRepositorio(DBVentaAngularContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Método para consultar productos con un filtro opcional
        public async Task<IQueryable<Producto>> Consultar(Expression<Func<Producto, bool>> filtro = null)
        {
            // Si no hay filtro, devuelve todos los productos; de lo contrario, aplica el filtro
            IQueryable<Producto> queryEntidad = filtro == null ? _dbContext.Productos : _dbContext.Productos.Where(filtro);
            return queryEntidad;
        }

        // Método para crear un nuevo producto
        public async Task<Producto> Crear(Producto entidad)
        {
            try
            {
                // Añade el nuevo producto al contexto
                _dbContext.Set<Producto>().Add(entidad);
                // Guarda los cambios en la base de datos de forma asincrónica
                await _dbContext.SaveChangesAsync();
                return entidad;
            }
            catch
            {
                // Si ocurre una excepción, se lanza para ser manejada por el llamador
                throw;
            }
        }

        // Método para editar un producto existente
        public async Task<bool> Editar(Producto entidad)
        {
            try
            {
                // Actualiza el producto en el contexto
                _dbContext.Update(entidad);
                // Guarda los cambios en la base de datos de forma asincrónica
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                // Si ocurre una excepción, se lanza para ser manejada por el llamador
                throw;
            }
        }

        // Método para eliminar un producto existente
        public async Task<bool> Eliminar(Producto entidad)
        {
            try
            {
                // Elimina el producto del contexto
                _dbContext.Remove(entidad);
                // Guarda los cambios en la base de datos de forma asincrónica
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                // Si ocurre una excepción, se lanza para ser manejada por el llamador
                throw;
            }
        }

        // Método para obtener un producto que cumpla con un filtro
        public async Task<Producto> Obtener(Expression<Func<Producto, bool>> filtro = null)
        {
            try
            {
                // Aplica el filtro y devuelve el primer producto que lo cumpla, o null si no hay ninguno
                return await _dbContext.Productos.Where(filtro).FirstOrDefaultAsync();
            }
            catch
            {
                // Si ocurre una excepción, se lanza para ser manejada por el llamador
                throw;
            }
        }
    }
}
