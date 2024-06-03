using SistemaVentaAngular.Models;
using System.Linq.Expressions;

public interface IUsuarioRepositorio
{
    Task<IQueryable<Usuario>> Consultar(Expression<Func<Usuario, bool>> filtro = null, string includeProperties = "");
    Task<Usuario> Crear(Usuario entidad);
    Task<bool> Editar(Usuario entidad);
    Task<bool> Eliminar(Usuario entidad);
    Task<List<Usuario>> Lista();
    Task<Usuario> Obtener(Expression<Func<Usuario, bool>> filtro = null, string includeProperties = "");
}
