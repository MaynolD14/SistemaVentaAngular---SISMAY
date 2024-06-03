using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaVentaAngular.DTOs;
using SistemaVentaAngular.Repository.Contratos;
using SistemaVentaAngular.Utilidades;

namespace SistemaVentaAngular.Controllers
{
    // Define la ruta base para este controlador como "api/DashBoard"
    [Route("api/[controller]")]
    [ApiController]
    public class DashBoardController : ControllerBase
    {
        // Inyección de dependencias para AutoMapper y el repositorio de dashboard
        private readonly IMapper _mapper;
        private readonly IDashBoardRepositorio _dashboardRepositorio;

        // Constructor para inicializar las dependencias inyectadas
        public DashBoardController(IDashBoardRepositorio dashboardRepositorio, IMapper mapper)
        {
            _mapper = mapper;
            _dashboardRepositorio = dashboardRepositorio;
        }

        // Endpoint HTTP GET para obtener el resumen del dashboard
        [HttpGet]
        [Route("Resumen")]
        public async Task<IActionResult> Resumen()
        {
            // Inicializa una respuesta genérica para el DashBoardDTO
            Response<DashBoardDTO> _response = new Response<DashBoardDTO>();

            try
            {
                // Crea una instancia del DTO del dashboard
                DashBoardDTO vmDashboard = new DashBoardDTO();

                // Asigna valores al DTO obteniéndolos del repositorio
                vmDashboard.TotalVentas = await _dashboardRepositorio.TotalVentasUltimaSemana();
                vmDashboard.TotalIngresos = await _dashboardRepositorio.TotalIngresosUltimaSemana();
                vmDashboard.TotalProductos = await _dashboardRepositorio.TotalProductos();

                // Crea una lista para almacenar las ventas de la última semana
                List<VentasSemanaDTO> listaVentasSemana = new List<VentasSemanaDTO>();

                // Recorre las ventas obtenidas del repositorio y las añade a la lista
                foreach (KeyValuePair<string, int> item in await _dashboardRepositorio.VentasUltimaSemana())
                {
                    listaVentasSemana.Add(new VentasSemanaDTO()
                    {
                        Fecha = item.Key, // Asigna la fecha del par clave-valor a la propiedad Fecha del DTO
                        Total = item.Value // Asigna el total de ventas del par clave-valor a la propiedad Total del DTO
                    });
                }

                // Asigna la lista de ventas al DTO del dashboard
                vmDashboard.VentasUltimaSemana = listaVentasSemana;

                // Configura la respuesta con el DTO del dashboard
                _response = new Response<DashBoardDTO>() { status = true, msg = "ok", value = vmDashboard };

                // Devuelve una respuesta HTTP 200 con el resultado
                return StatusCode(StatusCodes.Status200OK, _response);
            }
            catch (Exception ex)
            {
                // En caso de error, configura una respuesta HTTP 500 con el mensaje de error
                _response = new Response<DashBoardDTO>() { status = false, msg = ex.Message, value = null };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }
    }
}
