using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaVentaAngular.DTOs;
using SistemaVentaAngular.Repository.Contratos;
using SistemaVentaAngular.Utilidades;

namespace SistemaVentaAngular.Controllers
{
    // Define la ruta base para este controlador como "api/Categoria"
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        // Inyección de dependencias para AutoMapper y el repositorio de categorías
        private readonly IMapper _mapper;
        private readonly ICategoriaRepositorio _categoriaRepositorio;

        // Constructor para inicializar las dependencias inyectadas
        public CategoriaController(ICategoriaRepositorio categoriaRepositorio, IMapper mapper)
        {
            _mapper = mapper;
            _categoriaRepositorio = categoriaRepositorio;
        }

        // Endpoint HTTP GET para obtener la lista de categorías
        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {
            // Inicializa una respuesta genérica para la lista de CategoriasDTO, dicha respuesta está creada en la carpeta de Utilidades
            Response<List<CategoriaDTO>> _response = new Response<List<CategoriaDTO>>();

            try
            {
                // Crea una lista para almacenar las categorías
                List<CategoriaDTO> _listaCategorias = new List<CategoriaDTO>();

                // Mapea las categorías obtenidas del repositorio a la lista de CategoriaDTO
                _listaCategorias = _mapper.Map<List<CategoriaDTO>>(await _categoriaRepositorio.Lista());

                // Verifica si hay categorías en la lista y ajusta la respuesta en consecuencia
                if (_listaCategorias.Count > 0)
                    _response = new Response<List<CategoriaDTO>>() { status = true, msg = "ok", value = _listaCategorias };
                else
                    _response = new Response<List<CategoriaDTO>>() { status = false, msg = "sin resultados", value = null };

                // Devuelve una respuesta HTTP 200 con el resultado
                return StatusCode(StatusCodes.Status200OK, _response);
            }
            catch (Exception ex)
            {
                // En caso de error, devuelve una respuesta HTTP 500 con el mensaje de error
                _response = new Response<List<CategoriaDTO>>() { status = false, msg = ex.Message, value = null };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }
    }
}
