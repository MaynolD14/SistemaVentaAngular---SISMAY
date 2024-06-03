using Microsoft.EntityFrameworkCore;
using SistemaVentaAngular.Models;
using SistemaVentaAngular.Repository.Contratos;
using System.Linq.Expressions;

namespace SistemaVentaAngular.Repository.Implementacion
{
    // Implementación del repositorio de usuarios que sigue el contrato IUsuarioRepositorio
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        // Contexto de la base de datos inyectado
        private readonly DBVentaAngularContext _dbContext;

        // Constructor que inicializa el contexto de la base de datos mediante inyección de dependencias
        public UsuarioRepositorio(DBVentaAngularContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Método para consultar usuarios con un filtro opcional y propiedades incluidas opcionales
        public async Task<IQueryable<Usuario>> Consultar(Expression<Func<Usuario, bool>> filtro = null, string includeProperties = "")
        {
            // Si no hay filtro, devuelve todos los usuarios; de lo contrario, aplica el filtro
            IQueryable<Usuario> queryEntidad = filtro == null ? _dbContext.Usuarios : _dbContext.Usuarios.Where(filtro);

            // Incluye las propiedades relacionadas especificadas en includeProperties
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                queryEntidad = queryEntidad.Include(includeProperty);
            }

            return queryEntidad;
        }

        // Método para crear un nuevo usuario
        public async Task<Usuario> Crear(Usuario entidad)
        {
            try
            {
                // Añade el nuevo usuario al contexto
                _dbContext.Set<Usuario>().Add(entidad);
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

        // Método para editar un usuario existente
        public async Task<bool> Editar(Usuario entidad)
        {
            try
            {
                // Actualiza el usuario en el contexto
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

        // Método para eliminar un usuario existente
        public async Task<bool> Eliminar(Usuario entidad)
        {
            try
            {
                // Elimina el usuario del contexto
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

        // Método para obtener la lista de todos los usuarios
        public async Task<List<Usuario>> Lista()
        {
            try
            {
                // Obtiene todos los usuarios de la base de datos y los convierte a una lista asincrónicamente
                return await _dbContext.Usuarios.ToListAsync();
            }
            catch
            {
                // Si ocurre una excepción, se lanza para ser manejada por el llamador
                throw;
            }
        }

        // Método para obtener un usuario que cumpla con un filtro específico y propiedades incluidas opcionales
        public async Task<Usuario> Obtener(Expression<Func<Usuario, bool>> filtro = null, string includeProperties = "")
        {
            try
            {
                IQueryable<Usuario> query = _dbContext.Usuarios;

                // Aplica el filtro si se proporciona
                if (filtro != null)
                {
                    query = query.Where(filtro);
                }

                // Incluye las propiedades relacionadas especificadas en includeProperties
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }

                // Devuelve el primer usuario que cumpla con el filtro, o null si no hay ninguno
                return await query.FirstOrDefaultAsync();
            }
            catch
            {
                // Si ocurre una excepción, se lanza para ser manejada por el llamador
                throw;
            }
        }
    }
}
