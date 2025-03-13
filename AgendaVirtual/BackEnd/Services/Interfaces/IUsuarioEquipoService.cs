using BackEnd.DTO;

namespace BackEnd.Services.Interfaces
{
    public interface IUsuarioEquipoService
    {
        UsuarioEquipoDTO Get(int idUsuario, int idEquipo);
        List<UsuarioEquipoDTO> GetAll();
        UsuarioEquipoDTO Add(UsuarioEquipoDTO usuarioEquipoDTO);
        UsuarioEquipoDTO Update(UsuarioEquipoDTO usuarioEquipoDTO);
        UsuarioEquipoDTO Delete(int idUsuario, int idEquipo);
    }
}
