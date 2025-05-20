using DTO.SistemaVenta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.SistemaVenta.Servicios.Contrato
{
    public interface IProductoService
    {
        Task<List<ProductoDTO>> Lista();
        Task<ProductoDTO> Crear(ProductoDTO productoDTO);
        Task<bool> Editar(ProductoDTO productoDTO);
        Task<bool> Eliminar(int id);
    }
}
