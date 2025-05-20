using DTO.SistemaVenta;

namespace BLL.SistemaVenta.Servicios.Contrato
{
    public interface IUsuarioService
    {
        Task<List<UsuarioDTO>> Lista();
        Task<SesionDTO> ValidarCredenciales(string correo, string clave);
        Task<UsuarioDTO> Crear(UsuarioDTO usuarioDTO);
        Task<bool> Editar(UsuarioDTO usuarioDTO);
        Task<bool> Eliminar(int id);
    }
}
