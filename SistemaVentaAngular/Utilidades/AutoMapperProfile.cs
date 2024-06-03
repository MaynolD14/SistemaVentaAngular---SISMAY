using AutoMapper;
using SistemaVentaAngular.DTOs;
using SistemaVentaAngular.Models;
using System.Globalization;

namespace SistemaVentaAngular.Utilidades
{
    // Configuración de AutoMapper para la aplicación
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region Rol
            // Mapeo bidireccional entre Rol y RolDTO
            CreateMap<Rol, RolDTO>().ReverseMap();
            #endregion Rol

            #region Usuario
            // Mapeo de Usuario a UsuarioDTO con configuración adicional
            CreateMap<Usuario, UsuarioDTO>()
                // Mapea la propiedad RolDescripcion del destino obteniendo el valor de Descripcion de IdRolNavigation en el origen
                .ForMember(destino => destino.RolDescripcion,
                           opt => opt.MapFrom(origen => origen.IdRolNavigation.Descripcion));

            // Mapeo de UsuarioDTO a Usuario con configuración adicional
            CreateMap<UsuarioDTO, Usuario>()
                // Ignora la propiedad IdRolNavigation en el mapeo inverso
                .ForMember(destino => destino.IdRolNavigation,
                           opt => opt.Ignore());
            #endregion Usuario

            #region Categoria
            // Mapeo bidireccional entre Categoria y CategoriaDTO
            CreateMap<Categoria, CategoriaDTO>().ReverseMap();
            #endregion Categoria

            #region Producto
            // Mapeo de Producto a ProductoDTO con configuración adicional
            CreateMap<Producto, ProductoDTO>()
                // Mapea la propiedad DescripcionCategoria del destino obteniendo el valor de Descripcion de IdCategoriaNavigation en el origen
                .ForMember(destino =>
                    destino.DescripcionCategoria,
                    opt => opt.MapFrom(origen => origen.IdCategoriaNavigation.Descripcion)
                )
                // Convierte el valor de Precio de decimal a string con formato cultural "es-PE"
                .ForMember(destino =>
                    destino.Precio,
                    opt => opt.MapFrom(origen => Convert.ToString(origen.Precio.Value, new CultureInfo("es-PE")))
                );

            // Mapeo de ProductoDTO a Producto con configuración adicional
            CreateMap<ProductoDTO, Producto>()
                // Ignora la propiedad IdCategoriaNavigation en el mapeo inverso
                .ForMember(destino =>
                    destino.IdCategoriaNavigation,
                    opt => opt.Ignore()
                )
                // Convierte el valor de Precio de string a decimal con formato cultural "es-PE"
                .ForMember(destino =>
                    destino.Precio,
                    opt => opt.MapFrom(origen => Convert.ToDecimal(origen.Precio, new CultureInfo("es-PE")))
                );
            #endregion Producto

            #region Venta
            // Mapeo de Venta a VentaDTO con configuración adicional
            CreateMap<Venta, VentaDTO>()
                // Convierte el valor de Total de decimal a string con formato cultural "es-PE"
                .ForMember(destino =>
                    destino.TotalTexto,
                    opt => opt.MapFrom(origen => Convert.ToString(origen.Total.Value, new CultureInfo("es-PE")))
                )
                // Convierte el valor de FechaRegistro a string con formato "dd/MM/yyyy"
                .ForMember(destino =>
                    destino.FechaRegistro,
                    opt => opt.MapFrom(origen => origen.FechaRegistro.Value.ToString("dd/MM/yyyy"))
                );

            // Mapeo de VentaDTO a Venta con configuración adicional
            CreateMap<VentaDTO, Venta>()
                // Convierte el valor de TotalTexto de string a decimal con formato cultural "es-PE"
                .ForMember(destino =>
                    destino.Total,
                    opt => opt.MapFrom(origen => Convert.ToDecimal(origen.TotalTexto, new CultureInfo("es-PE")))
                );
            #endregion Venta

            #region DetalleVenta
            // Mapeo de DetalleVenta a DetalleVentaDTO con configuración adicional
            CreateMap<DetalleVenta, DetalleVentaDTO>()
                // Mapea la propiedad DescripcionProducto del destino obteniendo el valor de Nombre de IdProductoNavigation en el origen
                .ForMember(destino =>
                    destino.DescripcionProducto,
                    opt => opt.MapFrom(origen => origen.IdProductoNavigation.Nombre)
                )
                // Convierte el valor de Precio de decimal a string con formato cultural "es-PE"
                .ForMember(destino =>
                    destino.PrecioTexto,
                    opt => opt.MapFrom(origen => Convert.ToString(origen.Precio.Value, new CultureInfo("es-PE")))
                )
                // Convierte el valor de Total de decimal a string con formato cultural "es-PE"
                .ForMember(destino =>
                    destino.TotalTexto,
                    opt => opt.MapFrom(origen => Convert.ToString(origen.Total.Value, new CultureInfo("es-PE")))
                );

            // Mapeo de DetalleVentaDTO a DetalleVenta con configuración adicional
            CreateMap<DetalleVentaDTO, DetalleVenta>()
                // Convierte el valor de PrecioTexto de string a decimal con formato cultural "es-PE"
                .ForMember(destino =>
                    destino.Precio,
                    opt => opt.MapFrom(origen => Convert.ToDecimal(origen.PrecioTexto, new CultureInfo("es-PE")))
                )
                // Convierte el valor de TotalTexto de string a decimal con formato cultural "es-PE"
                .ForMember(destino =>
                    destino.Total,
                    opt => opt.MapFrom(origen => Convert.ToDecimal(origen.TotalTexto, new CultureInfo("es-PE")))
                );
            #endregion DetalleVenta

            #region Reporte
            // Mapeo de DetalleVenta a ReporteDTO con configuración adicional
            CreateMap<DetalleVenta, ReporteDTO>()
                // Convierte el valor de FechaRegistro a string con formato "dd/MM/yyyy"
                .ForMember(destino =>
                    destino.FechaRegistro,
                    opt => opt.MapFrom(origen => origen.IdVentaNavigation.FechaRegistro.Value.ToString("dd/MM/yyyy"))
                )
                // Mapea la propiedad NumeroDocumento del destino obteniendo el valor de NumeroDocumento de IdVentaNavigation en el origen
                .ForMember(destino =>
                    destino.NumeroDocumento,
                    opt => opt.MapFrom(origen => origen.IdVentaNavigation.NumeroDocumento)
                )
                // Mapea la propiedad TipoPago del destino obteniendo el valor de TipoPago de IdVentaNavigation en el origen
                .ForMember(destino =>
                    destino.TipoPago,
                    opt => opt.MapFrom(origen => origen.IdVentaNavigation.TipoPago)
                )
                // Convierte el valor de TotalVenta de decimal a string con formato cultural "es-PE"
                .ForMember(destino =>
                    destino.TotalVenta,
                    opt => opt.MapFrom(origen => Convert.ToString(origen.IdVentaNavigation.Total.Value, new CultureInfo("es-PE")))
                )
                // Mapea la propiedad Producto del destino obteniendo el valor de Nombre de IdProductoNavigation en el origen
                .ForMember(destino =>
                    destino.Producto,
                    opt => opt.MapFrom(origen => origen.IdProductoNavigation.Nombre)
                )
                // Convierte el valor de Precio de decimal a string con formato cultural "es-PE"
                .ForMember(destino =>
                    destino.Precio,
                    opt => opt.MapFrom(origen => Convert.ToString(origen.Precio.Value, new CultureInfo("es-PE")))
                )
                // Convierte el valor de Total de decimal a string con formato cultural "es-PE"
                .ForMember(destino =>
                    destino.Total,
                    opt => opt.MapFrom(origen => Convert.ToString(origen.Total.Value, new CultureInfo("es-PE")))
                );
            #endregion Reporte
        }
    }
}
