using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaVentaAngular.DTOs;
using SistemaVentaAngular.Models;
using SistemaVentaAngular.Repository.Contratos;
using SistemaVentaAngular.Utilidades;

namespace SistemaVentaAngular.Controllers
{
    // Define la ruta base para este controlador como "api/Producto"
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        // Inyección de dependencias para AutoMapper y el repositorio de productos
        private readonly IMapper _mapper;
        private readonly IProductoRepositorio _productoRepositorio;

        // Constructor para inicializar las dependencias inyectadas
        public ProductoController(IProductoRepositorio productoRepositorio, IMapper mapper)
        {
            _mapper = mapper;
            _productoRepositorio = productoRepositorio;
        }

        // Endpoint HTTP GET para obtener la lista de productos
        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {
            // Inicializa una respuesta genérica para la lista de ProductoDTO
            Response<List<ProductoDTO>> _response = new Response<List<ProductoDTO>>();

            try
            {
                // Crea una lista para almacenar los productos
                List<ProductoDTO> ListaProductos = new List<ProductoDTO>();

                // Obtiene los productos del repositorio y los incluye con la navegación de categorías
                IQueryable<Producto> query = await _productoRepositorio.Consultar();
                query = query.Include(r => r.IdCategoriaNavigation);

                // Mapea los productos obtenidos a la lista de ProductoDTO
                ListaProductos = _mapper.Map<List<ProductoDTO>>(query.ToList());

                // Verifica si hay productos en la lista y ajusta la respuesta en consecuencia
                if (ListaProductos.Count > 0)
                    _response = new Response<List<ProductoDTO>>() { status = true, msg = "ok", value = ListaProductos };
                else
                    _response = new Response<List<ProductoDTO>>() { status = false, msg = "", value = null };

                // Devuelve una respuesta HTTP 200 con el resultado
                return StatusCode(StatusCodes.Status200OK, _response);
            }
            catch (Exception ex)
            {
                // En caso de error, devuelve una respuesta HTTP 500 con el mensaje de error
                _response = new Response<List<ProductoDTO>>() { status = false, msg = ex.Message, value = null };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        // Endpoint HTTP POST para guardar un nuevo producto
        [HttpPost]
        [Route("Guardar")]
        public async Task<IActionResult> Guardar([FromBody] ProductoDTO request)
        {
            // Inicializa una respuesta genérica para el ProductoDTO
            Response<ProductoDTO> _response = new Response<ProductoDTO>();

            try
            {
                // Mapea el DTO recibido a una entidad de producto
                Producto _producto = _mapper.Map<Producto>(request);

                // Crea el nuevo producto en el repositorio
                Producto _productoCreado = await _productoRepositorio.Crear(_producto);

                // Verifica si el producto fue creado exitosamente y ajusta la respuesta en consecuencia
                if (_productoCreado.IdProducto != 0)
                    _response = new Response<ProductoDTO>() { status = true, msg = "ok", value = _mapper.Map<ProductoDTO>(_productoCreado) };
                else
                    _response = new Response<ProductoDTO>() { status = false, msg = "No se pudo crear el producto" };

                // Devuelve una respuesta HTTP 200 con el resultado
                return StatusCode(StatusCodes.Status200OK, _response);
            }
            catch (Exception ex)
            {
                // En caso de error, devuelve una respuesta HTTP 500 con el mensaje de error
                _response = new Response<ProductoDTO>() { status = false, msg = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        // Endpoint HTTP PUT para editar un producto existente
        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] ProductoDTO request)
        {
            // Inicializa una respuesta genérica para el ProductoDTO
            Response<ProductoDTO> _response = new Response<ProductoDTO>();

            try
            {
                // Mapea el DTO recibido a una entidad de producto
                Producto _producto = _mapper.Map<Producto>(request);

                // Obtiene el producto existente del repositorio
                Producto _productoParaEditar = await _productoRepositorio.Obtener(u => u.IdProducto == _producto.IdProducto);

                // Verifica si el producto fue encontrado
                if (_productoParaEditar != null)
                {
                    // Actualiza las propiedades del producto
                    _productoParaEditar.Nombre = _producto.Nombre;
                    _productoParaEditar.IdCategoria = _producto.IdCategoria;
                    _productoParaEditar.Stock = _producto.Stock;
                    _productoParaEditar.Precio = _producto.Precio;

                    // Edita el producto en el repositorio
                    bool respuesta = await _productoRepositorio.Editar(_productoParaEditar);

                    // Verifica si el producto fue editado exitosamente y ajusta la respuesta en consecuencia
                    if (respuesta)
                        _response = new Response<ProductoDTO>() { status = true, msg = "ok", value = _mapper.Map<ProductoDTO>(_productoParaEditar) };
                    else
                        _response = new Response<ProductoDTO>() { status = false, msg = "No se pudo editar el producto" };
                }
                else
                {
                    // Si no se encontró el producto, ajusta la respuesta en consecuencia
                    _response = new Response<ProductoDTO>() { status = false, msg = "No se encontró el producto" };
                }

                // Devuelve una respuesta HTTP 200 con el resultado
                return StatusCode(StatusCodes.Status200OK, _response);
            }
            catch (Exception ex)
            {
                // En caso de error, devuelve una respuesta HTTP 500 con el mensaje de error
                _response = new Response<ProductoDTO>() { status = false, msg = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        // Endpoint HTTP DELETE para eliminar un producto existente
        [HttpDelete]
        [Route("Eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            // Inicializa una respuesta genérica para una cadena (mensaje)
            Response<string> _response = new Response<string>();

            try
            {
                // Obtiene el producto a eliminar del repositorio
                Producto _productoEliminar = await _productoRepositorio.Obtener(u => u.IdProducto == id);

                // Verifica si el producto fue encontrado
                if (_productoEliminar != null)
                {
                    // Elimina el producto del repositorio
                    bool respuesta = await _productoRepositorio.Eliminar(_productoEliminar);

                    // Verifica si el producto fue eliminado exitosamente y ajusta la respuesta en consecuencia
                    if (respuesta)
                        _response = new Response<string>() { status = true, msg = "ok", value = "" };
                    else
                        _response = new Response<string>() { status = false, msg = "No se pudo eliminar el producto", value = "" };
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
