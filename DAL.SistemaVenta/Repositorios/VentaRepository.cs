using DAL.SistemaVenta.DBContext;
using DAL.SistemaVenta.Repositorios.Contrato;
using Model.SistemaVenta;

namespace DAL.SistemaVenta.Repositorios
{
    public class VentaRepository: GenericRepository<Venta>, IVentaRepository
    {
        private readonly DbventaContext _dbcontext;
        public VentaRepository(DbventaContext dbcontext): base(dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<Venta> RegistrarVenta(Venta modelo)
        {
            Venta ventaGenerada = new Venta();

            using (var transaction = _dbcontext.Database.BeginTransaction())
            {
                try
                {
                    foreach (DetalleVenta dv in modelo.DetalleVenta)
                    {
                        Producto productoEncontrado = _dbcontext.Productos.Where(p => p.IdProducto == dv.IdProducto).First();
                        productoEncontrado.Stock -= dv.Cantidad;
                        _dbcontext.Productos.Update(productoEncontrado);
                    }
                    await _dbcontext.SaveChangesAsync();

                    NumeroDocumento correlativo = _dbcontext.NumeroDocumentos.First();
                    correlativo.UltimoNumero += 1;
                    correlativo.FechaRegistro = DateTime.Now;

                    _dbcontext.NumeroDocumentos.Update(correlativo);
                    await _dbcontext.SaveChangesAsync();

                    int cantidadDigitos = 4;
                    string ceros = string.Concat(Enumerable.Repeat('0', cantidadDigitos));
                    string numeroVenta = ceros + correlativo.UltimoNumero.ToString();

                    numeroVenta = numeroVenta.Substring(numeroVenta.Length - cantidadDigitos, cantidadDigitos);

                    modelo.NumeroDocumento = numeroVenta;

                    await _dbcontext.Venta.AddAsync(modelo);
                    await _dbcontext.SaveChangesAsync();

                    ventaGenerada = modelo;

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Error al registrar la venta", ex);
                }
                return ventaGenerada;
            }           
        }
    }
}
