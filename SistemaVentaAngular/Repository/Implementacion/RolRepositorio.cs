using Microsoft.EntityFrameworkCore;
using SistemaVentaAngular.Models;
using SistemaVentaAngular.Repository.Contratos;
using System.Linq.Expressions;

namespace SistemaVentaAngular.Repository.Implementacion
{
    // Implementación del repositorio de roles que sigue el contrato IRolRepositorio
    public class RolRepositorio : IRolRepositorio
    {
        // Contexto de la base de datos inyectado
        private readonly DBVentaAngularContext _dbContext;

        // Constructor que inicializa el contexto de la base de datos mediante inyección de dependencias
        public RolRepositorio(DBVentaAngularContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Método para obtener la lista de roles de la base de datos
        public async Task<List<Rol>> Lista()
        {
            try
            {
                // Obtiene todos los roles de la base de datos y los convierte a una lista asincrónicamente
                return await _dbContext.Rols.ToListAsync();
            }
            catch
            {
                // Si ocurre una excepción, se lanza para ser manejada por el llamador
                throw;
            }
        }
    }
}
