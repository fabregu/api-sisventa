using AutoMapper;
using BLL.SistemaVenta.Servicios.Contrato;
using DAL.SistemaVenta.Repositorios.Contrato;
using DTO.SistemaVenta;
using Microsoft.EntityFrameworkCore;
using Model.SistemaVenta;

namespace BLL.SistemaVenta.Servicios
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IGenericRepository<Usuario> _usuarioRepository;
        private readonly IMapper _mapper;
        public UsuarioService(IGenericRepository<Usuario> usuarioRepository, IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
        }
        public async Task<List<UsuarioDTO>> Lista()
        {
            try
            {
                var queryUsuarios= await _usuarioRepository.Consultar();
                var listaUsuarios = queryUsuarios.Include(rol => rol.IdRolNavigation).ToList();
                return _mapper.Map<List<UsuarioDTO>>(listaUsuarios);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de usuarios", ex);
            }
        }

        public async Task<SesionDTO> ValidarCredenciales(string correo, string clave)
        {
            try
            {
                var queryUsuario = await _usuarioRepository.Consultar(u =>
                    u.Correo == correo &&
                    u.Clave == clave);
                if (queryUsuario.FirstOrDefault() == null)
                    throw new TaskCanceledException("El usuario no existe");
                Usuario devolverUsuario = queryUsuario.Include(rol => rol.IdRolNavigation).First();
                return _mapper.Map<SesionDTO>(devolverUsuario);

            }
            catch (Exception ex)
            {
                throw new Exception("Error al validar las credenciales", ex);
            }
        }

        public async Task<UsuarioDTO> Crear(UsuarioDTO usuarioDTO)
        {
            try
            {
                var usuarioCreado = await _usuarioRepository.Crear(_mapper.Map<Usuario>(usuarioDTO));
                if (usuarioCreado.IdUsuario == 0)
                    throw new TaskCanceledException("El usuario no fue creado");

                var query = await _usuarioRepository.Consultar(u => u.IdUsuario == usuarioCreado.IdUsuario);
                usuarioCreado = query.Include(rol => rol.IdRolNavigation).First();

                return _mapper.Map<UsuarioDTO>(usuarioCreado);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear el usuario", ex);
            }
        }

        public async Task<bool> Editar(UsuarioDTO usuarioDTO)
        {
            try
            {
                var usuario = _mapper.Map<Usuario>(usuarioDTO);
                var usuarioEncontrado = await _usuarioRepository.Obtener(u => u.IdUsuario == usuarioDTO.IdUsuario);

                if (usuarioEncontrado == null)
                    throw new TaskCanceledException("El usuario no existe");

                usuarioEncontrado.NombreCompleto = usuario.NombreCompleto;
                usuarioEncontrado.Correo = usuario.Correo;
                usuarioEncontrado.IdRol = usuario.IdRol;
                usuarioEncontrado.Clave = usuario.Clave;
                usuarioEncontrado.EsActivo = usuario.EsActivo;

                bool respuesta = await _usuarioRepository.Editar(usuarioEncontrado);

                if (!respuesta)
                    throw new TaskCanceledException("El usuario no fue editado");
                return respuesta;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al editar el usuario", ex);
            }

        }

        public async Task<bool> Eliminar(int id)
        {
            try
            {
                var usuario = await _usuarioRepository.Obtener(u => u.IdUsuario == id);
                if (usuario == null)
                    throw new TaskCanceledException("El usuario no existe");
                bool respuesta = await _usuarioRepository.Eliminar(usuario);
                if (!respuesta)
                    throw new TaskCanceledException("No se pudo eliminar");
                return respuesta;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el usuario", ex);
            }
        }
    }
}
