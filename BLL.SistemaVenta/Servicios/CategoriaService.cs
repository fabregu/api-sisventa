using AutoMapper;
using BLL.SistemaVenta.Servicios.Contrato;
using DAL.SistemaVenta.Repositorios.Contrato;
using DTO.SistemaVenta;
using Model.SistemaVenta;

namespace BLL.SistemaVenta.Servicios
{
    public class CategoriaService : ICategoriaService
    {
        private readonly IGenericRepository<Categoria> _categoriaRepository;
        private readonly IMapper _mapper;
        public CategoriaService(IGenericRepository<Categoria> categoriaRepository, IMapper mapper)
        {
            _categoriaRepository = categoriaRepository;
            _mapper = mapper;
        }

        public async Task<List<CategoriaDTO>> Lista()
        {
            try
            {
                var listaCategorias = await _categoriaRepository.Consultar();
                return _mapper.Map<List<CategoriaDTO>>(listaCategorias.ToList());
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las categorías", ex);
            }
        }
    }
}
