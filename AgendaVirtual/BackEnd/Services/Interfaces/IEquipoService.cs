using BackEnd.DTO;

namespace BackEnd.Services.Interfaces
{
    public interface IEquipoService
    {
        EquipoDTO Get(int id);
        List<EquipoDTO> GetEquipo();
        void AddEquipo(EquipoDTO equipo);
        EquipoDTO Update(EquipoDTO equipoDTO);
        EquipoDTO Delete(int id);
        List<EquipoDTO> GetAllByUser(int idUsuario);
        List<UsuarioDTO> GetUsuariosByEquipo(int idEquipo);
        List<UsuarioDTO> GetUsuariosNotIntEquipo(int idEquipo);
        EquipoDTO GetEquipoPorUsuario(int idUsuario);
    }
}