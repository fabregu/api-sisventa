using AutoMapper;
using BLL.SistemaVenta.Servicios.Contrato;
using DAL.SistemaVenta.Repositorios.Contrato;
using DTO.SistemaVenta;
using Microsoft.EntityFrameworkCore;
using Model.SistemaVenta;
using System.Globalization;

namespace BLL.SistemaVenta.Servicios
{
    public class VentaService : IVentaService
    {
        private readonly IVentaRepository _ventaRepository;
        private readonly IGenericRepository<DetalleVenta> _detalleVentaRepository;
        private readonly IMapper _mapper;
        public VentaService(IVentaRepository ventaRepository, IGenericRepository<DetalleVenta> detalleVentaRepository, IMapper mapper)
        {
            _ventaRepository = ventaRepository;
            _detalleVentaRepository = detalleVentaRepository;
            _mapper = mapper;
        }

        public async Task<VentaDTO> Registrar(VentaDTO ventaDTO)
        {
            try
            {
                var ventaGenerada = await _ventaRepository.RegistrarVenta(_mapper.Map<Venta>(ventaDTO));
                if (ventaGenerada.IdVenta == 0)
                    throw new TaskCanceledException("Error al crear la venta");
                return _mapper.Map<VentaDTO>(ventaGenerada);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear la venta", ex);
            }
        }
        public async Task<List<VentaDTO>> Historial(string buscarPor, string numeroVenta, string fechaInicio, string fechaFin)
        {
            IQueryable<Venta> query = await _ventaRepository.Consultar();

            var listaResultado = new List<Venta>();
            try
            {
                if (buscarPor == "fecha")
                {
                    DateTime fecha_inicio = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", new CultureInfo("es-PE"));
                    DateTime fecha_fin = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", new CultureInfo("es-PE"));

                    listaResultado = await query.Where(v =>
                        v.FechaRegistro.Value.Date >= fecha_inicio.Date &&
                        v.FechaRegistro.Value.Date <= fecha_fin.Date
                        ).Include(dv => dv.DetalleVenta)
                        .ThenInclude(p => p.IdProductoNavigation)
                        .ToListAsync();
                }
                else
                {
                    listaResultado = await query.Where(v => v.NumeroDocumento == numeroVenta
                         ).Include(dv => dv.DetalleVenta)
                         .ThenInclude(p => p.IdProductoNavigation)
                         .ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el historial de ventas", ex);
            }
            return _mapper.Map<List<VentaDTO>>(listaResultado);
        }              

        public async Task<List<ReporteDTO>> Reporte(string fechainicio, string fechaFin)
        {
            IQueryable<DetalleVenta> query = await _detalleVentaRepository.Consultar();
            var listaResultado = new List<DetalleVenta>();
            try
            {
                DateTime fecha_inicio = DateTime.ParseExact(fechainicio, "dd/MM/yyyy", new CultureInfo("es-PE"));
                DateTime fecha_fin = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", new CultureInfo("es-PE"));

                listaResultado = await query
                    .Include(p => p.IdProductoNavigation)
                    .Include(v => v.IdVentaNavigation)
                    .Where(dv => 
                        dv.IdVentaNavigation.FechaRegistro.Value.Date >= fecha_inicio.Date &&
                        dv.IdVentaNavigation.FechaRegistro.Value.Date <= fecha_fin.Date)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el reporte de ventas", ex);
            }
            return _mapper.Map<List<ReporteDTO>>(listaResultado);
        }
    }
}
