using Model.SistemaVenta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.SistemaVenta.Repositorios.Contrato
{
    public interface IVentaRepository: IGenericRepository<Venta>
    {
        Task<Venta> RegistrarVenta(Venta modelo);
    }
}
