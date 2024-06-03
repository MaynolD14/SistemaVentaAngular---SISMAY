using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaVentaAngular.DTOs;
using SistemaVentaAngular.Repository.Contratos;
using AutoMapper;
using SistemaVentaAngular.Models;
using Microsoft.EntityFrameworkCore;
using SistemaVentaAngular.Utilidades;

namespace SistemaVentaAngular.Controllers
{
    // Define la ruta base para este controlador como "api/Usuario"
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        // Inyección de dependencias para AutoMapper y el repositorio de usuarios
        private readonly IMapper _mapper;
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        // Constructor para inicializar las dependencias inyectadas
        public UsuarioController(IUsuarioRepositorio usuarioRepositorio, IMapper mapper)
        {
            _mapper = mapper;
            _usuarioRepositorio = usuarioRepositorio;
        }

        // Endpoint HTTP GET para obtener la lista de usuarios
        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {
            // Inicializa una respuesta genérica para la lista de UsuarioDTO
            Response<List<UsuarioDTO>> _response = new Response<List<UsuarioDTO>>();

            try
            {
                // Crea una lista para almacenar los usuarios
                List<UsuarioDTO> ListaUsuarios = new List<UsuarioDTO>();

                // Obtiene los usuarios del repositorio y los incluye con la navegación de roles
                IQueryable<Usuario> query = await _usuarioRepositorio.Consultar();
                query = query.Include(r => r.IdRolNavigation);

                // Mapea los usuarios obtenidos a la lista de UsuarioDTO
                ListaUsuarios = _mapper.Map<List<UsuarioDTO>>(query.ToList());

                // Verifica si hay usuarios en la lista y ajusta la respuesta en consecuencia
                if (ListaUsuarios.Count > 0)
                    _response = new Response<List<UsuarioDTO>>() { status = true, msg = "ok", value = ListaUsuarios };
                else
                    _response = new Response<List<UsuarioDTO>>() { status = false, msg = "", value = null };

                // Devuelve una respuesta HTTP 200 con el resultado
                return StatusCode(StatusCodes.Status200OK, _response);
            }
            catch (Exception ex)
            {
                // En caso de error, devuelve una respuesta HTTP 500 con el mensaje de error
                _response = new Response<List<UsuarioDTO>>() { status = false, msg = ex.Message, value = null };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        // Endpoint HTTP GET para iniciar sesión
        [HttpGet]
        [Route("IniciarSesion")]
        public async Task<IActionResult> IniciarSesion(string correo, string clave)
        {
            // Inicializa una respuesta genérica para un objeto dinámico
            Response<dynamic> _response = new Response<dynamic>();

            try
            {
                // Obtiene el usuario del repositorio basado en el correo y clave proporcionados
                Usuario _usuario = await _usuarioRepositorio.Obtener(u => u.Correo == correo && u.Clave == clave, "IdRolNavigation");

                // Verifica si el usuario fue encontrado
                if (_usuario != null)
                {
                    // Mapea el usuario a un DTO
                    var usuarioDTO = _mapper.Map<UsuarioDTO>(_usuario);
                    _response = new Response<dynamic>() { status = true, msg = "ok", value = new { usuario = usuarioDTO, rol = usuarioDTO.RolDescripcion } };
                }
                else
                {
                    _response = new Response<dynamic>() { status = false, msg = "No se encontró el usuario", value = null };
                }

                // Devuelve una respuesta HTTP 200 con el resultado
                return StatusCode(StatusCodes.Status200OK, _response);
            }
            catch (Exception ex)
            {
                // En caso de error, devuelve una respuesta HTTP 500 con el mensaje de error
                _response = new Response<dynamic>() { status = false, msg = ex.Message, value = null };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        // Endpoint HTTP POST para guardar un nuevo usuario
        [HttpPost]
        [Route("Guardar")]
        public async Task<IActionResult> Guardar([FromBody] UsuarioDTO request)
        {
            // Inicializa una respuesta genérica para el UsuarioDTO
            Response<UsuarioDTO> _response = new Response<UsuarioDTO>();

            try
            {
                // Mapea el DTO recibido a una entidad de usuario
                Usuario _usuario = _mapper.Map<Usuario>(request);

                // Crea el nuevo usuario en el repositorio
                Usuario _usuarioCreado = await _usuarioRepositorio.Crear(_usuario);

                // Verifica si el usuario fue creado exitosamente y ajusta la respuesta en consecuencia
                if (_usuarioCreado.IdUsuario != 0)
                    _response = new Response<UsuarioDTO>() { status = true, msg = "ok", value = _mapper.Map<UsuarioDTO>(_usuarioCreado) };
                else
                    _response = new Response<UsuarioDTO>() { status = false, msg = "No se pudo crear el usuario" };

                // Devuelve una respuesta HTTP 200 con el resultado
                return StatusCode(StatusCodes.Status200OK, _response);
            }
            catch (Exception ex)
            {
                // En caso de error, devuelve una respuesta HTTP 500 con el mensaje de error
                _response = new Response<UsuarioDTO>() { status = false, msg = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        // Endpoint HTTP PUT para editar un usuario existente
        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] UsuarioDTO request)
        {
            // Inicializa una respuesta genérica para el UsuarioDTO
            Response<UsuarioDTO> _response = new Response<UsuarioDTO>();

            try
            {
                // Mapea el DTO recibido a una entidad de usuario
                Usuario _usuario = _mapper.Map<Usuario>(request);

                // Obtiene el usuario existente del repositorio
                Usuario _usuarioParaEditar = await _usuarioRepositorio.Obtener(u => u.IdUsuario == _usuario.IdUsuario);

                // Verifica si el usuario fue encontrado
                if (_usuarioParaEditar != null)
                {
                    // Actualiza las propiedades del usuario
                    _usuarioParaEditar.NombreApellidos = _usuario.NombreApellidos;
                    _usuarioParaEditar.Correo = _usuario.Correo;
                    _usuarioParaEditar.IdRol = _usuario.IdRol;
                    _usuarioParaEditar.Clave = _usuario.Clave;

                    // Edita el usuario en el repositorio
                    bool respuesta = await _usuarioRepositorio.Editar(_usuarioParaEditar);

                    // Verifica si el usuario fue editado exitosamente y ajusta la respuesta en consecuencia
                    if (respuesta)
                        _response = new Response<UsuarioDTO>() { status = true, msg = "ok", value = _mapper.Map<UsuarioDTO>(_usuarioParaEditar) };
                    else
                        _response = new Response<UsuarioDTO>() { status = false, msg = "No se pudo editar el usuario" };
                }
                else
                {
                    // Si no se encontró el usuario, ajusta la respuesta en consecuencia
                    _response = new Response<UsuarioDTO>() { status = false, msg = "No se encontró el usuario" };
                }

                // Devuelve una respuesta HTTP 200 con el resultado
                return StatusCode(StatusCodes.Status200OK, _response);
            }
            catch (Exception ex)
            {
                // En caso de error, devuelve una respuesta HTTP 500 con el mensaje de error
                _response = new Response<UsuarioDTO>() { status = false, msg = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        // Endpoint HTTP DELETE para eliminar un usuario existente
        [HttpDelete]
        [Route("Eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            // Inicializa una respuesta genérica para una cadena (mensaje)
            Response<string> _response = new Response<string>();

            try
            {
                // Obtiene el usuario a eliminar del repositorio
                Usuario _usuarioEliminar = await _usuarioRepositorio.Obtener(u => u.IdUsuario == id);

                // Verifica si el usuario fue encontrado
                if (_usuarioEliminar != null)
                {
                    // Elimina el usuario del repositorio
                    bool respuesta = await _usuarioRepositorio.Eliminar(_usuarioEliminar);

                    // Verifica si el usuario fue eliminado exitosamente y ajusta la respuesta en consecuencia
                    if (respuesta)
                        _response = new Response<string>() { status = true, msg = "ok", value = "" };
                    else
                        _response = new Response<string>() { status = false, msg = "No se pudo eliminar el usuario", value = "" };
                }

                // Devuelve una respuesta HTTP 200 con el resultado
                return StatusCode(StatusCodes.Status200OK, _response);
            }
            catch (Exception ex)
            {
                // En caso de error, devuelve una respuesta HTTP 500 con el mensaje de error
                _response = new Response<string>() { status = false, msg = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }
    }
}
