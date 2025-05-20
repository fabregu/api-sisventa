using DTO.SistemaVenta;

namespace BLL.SistemaVenta.Servicios.Contrato
{
    public interface IDashboardService
    {
        Task<DashboardDTO> Resumen();
    }
}
