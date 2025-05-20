using DTO.SistemaVenta;

namespace BLL.SistemaVenta.Servicios.Contrato
{
    public interface IVentaService
    {
        Task<VentaDTO> Registrar(VentaDTO ventaDTO);
        Task<List<VentaDTO>> Historial( string buscarPor, string numeroVenta, string fechainicio, string fechaFin);
        Task<List<VentaDTO>> Reporte( string fechainicio, string fechaFin);        
    }
}
