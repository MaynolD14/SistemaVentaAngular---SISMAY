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
    // Define la ruta base para este controlador como "api/Rol"
    [Route("api/[controller]")]
    [ApiController]
    public class RolController : ControllerBase
    {
        // Inyección de dependencias para AutoMapper y el repositorio de roles
        private readonly IMapper _mapper;
        private readonly IRolRepositorio _rolRepositorio;

        // Constructor para inicializar las dependencias inyectadas
        public RolController(IRolRepositorio rolRepositorio, IMapper mapper)
        {
            _mapper = mapper;
            _rolRepositorio = rolRepositorio;
        }

        // Endpoint HTTP GET para obtener la lista de roles
        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {
            // Inicializa una respuesta genérica para la lista de RolDTO
            Response<List<RolDTO>> _response = new Response<List<RolDTO>>();

            try
            {
                // Crea una lista para almacenar los roles
                List<RolDTO> _listaRoles = new List<RolDTO>();

                // Obtiene la lista de roles del repositorio y los mapea a DTOs
                _listaRoles = _mapper.Map<List<RolDTO>>(await _rolRepositorio.Lista());

                // Verifica si hay roles en la lista y ajusta la respuesta en consecuencia
                if (_listaRoles.Count > 0)
                    _response = new Response<List<RolDTO>>() { status = true, msg = "ok", value = _listaRoles };
                else
                    _response = new Response<List<RolDTO>>() { status = false, msg = "sin resultados", value = null };

                // Devuelve una respuesta HTTP 200 con el resultado
                return StatusCode(StatusCodes.Status200OK, _response);
            }
            catch (Exception ex)
            {
                // En caso de error, devuelve una respuesta HTTP 500 con el mensaje de error
                _response = new Response<List<RolDTO>>() { status = false, msg = ex.Message, value = null };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }
    }
}
