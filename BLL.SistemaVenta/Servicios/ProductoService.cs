using AutoMapper;
using BLL.SistemaVenta.Servicios.Contrato;
using DAL.SistemaVenta.Repositorios.Contrato;
using DTO.SistemaVenta;
using Microsoft.EntityFrameworkCore;
using Model.SistemaVenta;

namespace BLL.SistemaVenta.Servicios
{
    public class ProductoService: IProductoService
    {
        private readonly IGenericRepository<Producto> _productoRepository;
        private readonly IMapper _mapper;
        public ProductoService(IGenericRepository<Producto> productoRepository, IMapper mapper)
        {
            _productoRepository = productoRepository;
            _mapper = mapper;
        }
        public async Task<List<ProductoDTO>> Lista()
        {
            try
            {
                var queryProducto = await _productoRepository.Consultar();
                var listaProductos = queryProducto.Include(cat => cat.IdCategoriaNavigation).ToList();
                return _mapper.Map<List<ProductoDTO>>(listaProductos.ToList());
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los productos", ex);
            }
        }
        public async Task<ProductoDTO> Crear(ProductoDTO productoDTO)
        {
            try
            {
                var productoCreado = await _productoRepository.Crear(_mapper.Map<Producto>(productoDTO));
                if (productoCreado.IdProducto == 0)                
                    throw new TaskCanceledException("Error al crear el producto");

                return _mapper.Map<ProductoDTO>(productoCreado);
                
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear el producto", ex);

            }
        }

        public async Task<bool> Editar(ProductoDTO productoDTO)
        {
            try
            {
                var producto = _mapper.Map<Producto>(productoDTO);
                var productoEncontrado = await _productoRepository.Obtener(u =>
                    u.IdProducto == producto.IdProducto);
                if (productoEncontrado == null)
                    throw new TaskCanceledException("El producto no existe");

                productoEncontrado.Nombre = producto.Nombre;
                productoEncontrado.IdCategoria = producto.IdCategoria;
                productoEncontrado.Stock = producto.Stock;
                productoEncontrado.Precio = producto.Precio;
                productoEncontrado.EsActivo = producto.EsActivo;

                bool respuesta = await _productoRepository.Editar(productoEncontrado);

                if (!respuesta)
                    throw new TaskCanceledException("No se pudo editar el producto");

                return respuesta;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al editar el producto", ex);
            }
        }

        public async Task<bool> Eliminar(int id)
        {
            try
            {
                var productoEncontrado = await _productoRepository.Obtener(p => p.IdProducto == id);
                if (productoEncontrado == null)
                    throw new TaskCanceledException("El producto no existe");

                bool respuesta = await _productoRepository.Eliminar(productoEncontrado);

                if (!respuesta)
                    throw new TaskCanceledException("No se pudo eliminar el producto");
                return respuesta;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el producto", ex);
            }
        }

    }
}
