using BackEnd.DTO;

namespace BackEnd.Services.Interfaces
{
    public interface IUsuarioService
    {
        UsuarioDTO Get(int id);
        List<UsuarioDTO> GetAll();
        UsuarioDTO Add(UsuarioDTO usuarioDTO);
        UsuarioDTO Update(UsuarioDTO usuarioDTO);
        UsuarioDTO Delete(int id);
    }
}
