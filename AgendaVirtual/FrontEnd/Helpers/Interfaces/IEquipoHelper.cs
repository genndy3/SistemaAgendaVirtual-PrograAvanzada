using FrontEnd.Models;

namespace FrontEnd.Helpers.Interfaces
{
    public interface IEquipoHelper
    {
        string Token { get; set; }
        List<EquipoViewModel> getEquipos();
        EquipoViewModel getEquipo(int id);
        EquipoViewModel addEquipo(EquipoViewModel equipo);
        EquipoViewModel updateEquipo(EquipoViewModel equipo);
        void deleteEquipo(int id);

        List<EquipoViewModel> getEquiposPorUsuario(int usuarioId);
        List<UsuarioViewModel> getUsuariosPorEquipo(int equipoId);
        List<UsuarioViewModel> getUsuariosNotInEquipo(int equipoId);

        EquipoViewModel GetEquipoPorUsuario(int usuarioId);
    }
}