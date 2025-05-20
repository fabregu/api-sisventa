using AutoMapper;
using BLL.SistemaVenta.Servicios.Contrato;
using DAL.SistemaVenta.Repositorios.Contrato;
using DTO.SistemaVenta;
using Model.SistemaVenta;

namespace BLL.SistemaVenta.Servicios
{
    public class MenuService: IMenuService
    {
        private readonly IGenericRepository<Usuario> _usuarioRepository;
        private readonly IGenericRepository<MenuRol> _menuRolRepository;
        private readonly IGenericRepository<Menu> _menuRepository;
        private readonly IMapper _mapper;
        public MenuService(IGenericRepository<Usuario> usuarioRepository, 
                           IGenericRepository<MenuRol> menuRolRepository, 
                           IGenericRepository<Menu> menuRepository, 
                           IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _menuRolRepository = menuRolRepository;
            _menuRepository = menuRepository;
            _mapper = mapper;
        }

        public async Task<List<MenuDTO>> Lista(int idUsuario)
        {
            IQueryable<Usuario> tblusuario = await _usuarioRepository.Consultar(u => u.IdUsuario == idUsuario);
            IQueryable<MenuRol> tblMenuRol = await _menuRolRepository.Consultar();
            IQueryable<Menu> tblMenu = await _menuRepository.Consultar();

            try
            {
                IQueryable<Menu> tblResultado = (from u in tblusuario
                                                 join mr in tblMenuRol on u.IdRol equals mr.IdRol
                                                 join m in tblMenu on mr.IdMenu equals m.IdMenu
                                                 select m).AsQueryable();
                var listaMenu = tblResultado.ToList();
                return _mapper.Map<List<MenuDTO>>(listaMenu);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
