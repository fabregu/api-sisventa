using DTO.SistemaVenta;

namespace BLL.SistemaVenta.Servicios.Contrato
{
    public interface ICategoriaService
    {
        Task<List<CategoriaDTO>> Lista();
    }
}
