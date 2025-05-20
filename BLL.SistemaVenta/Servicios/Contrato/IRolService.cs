using DTO.SistemaVenta;

namespace BLL.SistemaVenta.Servicios.Contrato
{
    public interface IRolService
    {
        Task<List<RolDTO>> Lista();
    }
}
