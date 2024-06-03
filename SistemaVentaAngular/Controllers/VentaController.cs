using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaVentaAngular.DTOs;
using SistemaVentaAngular.Models;
using SistemaVentaAngular.Repository.Contratos;
using SistemaVentaAngular.Utilidades;
using System.Globalization;

namespace SistemaVentaAngular.Controllers
{
    // Define la ruta base para este controlador como "api/Venta"
    [Route("api/[controller]")]
    [ApiController]
    public class VentaController : ControllerBase
    {
        // Inyección de dependencias para AutoMapper y el repositorio de ventas
        private readonly IMapper _mapper;
        private readonly IVentaRepositorio _ventaRepositorio;

        // Constructor para inicializar las dependencias inyectadas
        public VentaController(IVentaRepositorio ventaRepositorio, IMapper mapper)
        {
            _mapper = mapper;
            _ventaRepositorio = ventaRepositorio;
        }

        // Endpoint HTTP POST para registrar una nueva venta
        [HttpPost]
        [Route("Registrar")]
        public async Task<IActionResult> Registrar([FromBody] VentaDTO request)
        {
            // Inicializa una respuesta genérica para el VentaDTO
            Response<VentaDTO> _response = new Response<VentaDTO>();

            try
            {
                // Mapea el DTO recibido a una entidad de venta y la registra en el repositorio
                Venta venta_creada = await _ventaRepositorio.Registrar(_mapper.Map<Venta>(request));

                // Mapea la entidad de venta creada de nuevo a un DTO
                request = _mapper.Map<VentaDTO>(venta_creada);

                // Verifica si la venta fue registrada exitosamente y ajusta la respuesta en consecuencia
                if (venta_creada.IdVenta != 0)
                    _response = new Response<VentaDTO>() { status = true, msg = "ok", value = request };
                else
                    _response = new Response<VentaDTO>() { status = false, msg = "No se pudo registrar la venta" };

                // Devuelve una respuesta HTTP 200 con el resultado
                return StatusCode(StatusCodes.Status200OK, _response);
            }
            catch (Exception ex)
            {
                // En caso de error, devuelve una respuesta HTTP 500 con el mensaje de error
                _response = new Response<VentaDTO>() { status = false, msg = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        // Endpoint HTTP GET para obtener el historial de ventas
        [HttpGet]
        [Route("Historial")]
        public async Task<IActionResult> Historial(string buscarPor, string? numeroVenta, string? fechaInicio, string? fechaFin)
        {
            // Inicializa una respuesta genérica para la lista de VentaDTO
            Response<List<VentaDTO>> _response = new Response<List<VentaDTO>>();

            // Asigna valores predeterminados si los parámetros son nulos para que no quede nada vacío 
            numeroVenta = numeroVenta ?? "";
            fechaInicio = fechaInicio ?? "";
            fechaFin = fechaFin ?? "";

            try
            {
                // Obtiene el historial de ventas del repositorio y lo mapea a una lista de VentaDTO
                List<VentaDTO> vmHistorialVenta = _mapper.Map<List<VentaDTO>>(await _ventaRepositorio.Historial(buscarPor, numeroVenta, fechaInicio, fechaFin));

                // Verifica si hay ventas en el historial y ajusta la respuesta en consecuencia
                if (vmHistorialVenta.Count > 0)
                    _response = new Response<List<VentaDTO>>() { status = true, msg = "ok", value = vmHistorialVenta };
                else
                    _response = new Response<List<VentaDTO>>() { status = false, msg = "No se encontraron ventas", value = null };

                // Devuelve una respuesta HTTP 200 con el resultado
                return StatusCode(StatusCodes.Status200OK, _response);
            }
            catch (Exception ex)
            {
                // En caso de error, devuelve una respuesta HTTP 500 con el mensaje de error
                _response = new Response<List<VentaDTO>>() { status = false, msg = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        // Endpoint HTTP GET para obtener un reporte de ventas
        [HttpGet]
        [Route("Reporte")]
        public async Task<IActionResult> Reporte(string? fechaInicio, string? fechaFin)
        {
            // Inicializa una respuesta genérica para la lista de ReporteDTO
            Response<List<ReporteDTO>> _response = new Response<List<ReporteDTO>>();

            // Convierte las fechas de cadena a DateTime utilizando el formato específico (QUE PROBLEMA CON ESTA SHIT!)
            DateTime _fechaInicio = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", new CultureInfo("es-PE"));
            DateTime _fechaFin = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", new CultureInfo("es-PE"));

            try
            {
                // Obtiene el reporte de ventas del repositorio y lo mapea a una lista de ReporteDTO
                List<ReporteDTO> listaReporte = _mapper.Map<List<ReporteDTO>>(await _ventaRepositorio.Reporte(_fechaInicio, _fechaFin));

                // Verifica si hay datos en el reporte y ajusta la respuesta en consecuencia
                if (listaReporte.Count > 0)
                    _response = new Response<List<ReporteDTO>>() { status = true, msg = "ok", value = listaReporte };
                else
                    _response = new Response<List<ReporteDTO>>() { status = false, msg = "No se encontraron ventas en el periodo especificado", value = null };

                // Devuelve una respuesta HTTP 200 con el resultado
                return StatusCode(StatusCodes.Status200OK, _response);
            }
            catch (Exception ex)
            {
                // En caso de error, devuelve una respuesta HTTP 500 con el mensaje de error
                _response = new Response<List<ReporteDTO>>() { status = false, msg = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }
    }
}
