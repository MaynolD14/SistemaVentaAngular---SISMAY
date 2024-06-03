using Microsoft.EntityFrameworkCore;
using SistemaVentaAngular.Models;
using SistemaVentaAngular.Repository.Contratos;

namespace SistemaVentaAngular.Repository.Implementacion
{
    // Implementación del repositorio de categorías que sigue el contrato ICategoriaRepositorio
    public class CategoriaRepositorio : ICategoriaRepositorio
    {
        // Contexto de la base de datos inyectado
        private readonly DBVentaAngularContext _dbContext;

        // Constructor que inicializa el contexto de la base de datos mediante inyección de dependencias
        public CategoriaRepositorio(DBVentaAngularContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Método para obtener la lista de categorías de la base de datos
        public async Task<List<Categoria>> Lista()
        {
            try
            {
                // Obtiene todas las categorías de la base de datos y las convierte a una lista asincrónicamente
                return await _dbContext.Categoria.ToListAsync();
            }
            catch
            {
                // Si ocurre una excepción, se lanza para ser manejada por el llamador
                throw;
            }
        }
    }
}
