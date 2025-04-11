using FrontEnd.Models;

namespace FrontEnd.Helpers.Interfaces
{
    public interface IEquipoHelper
    {
        string Token { get; set; }
        

        List<EquipoViewModel> GetEquipos();
        EquipoViewModel GetById(int id);
        EquipoViewModel Add(EquipoViewModel equipo);
        EquipoViewModel EditEquipo(EquipoViewModel equipo);
        void DeleteEquipo(int id);

        List<EquipoViewModel> GetEquiposPorUsuario(int usuarioId);
        List<UsuarioViewModel> GetUsuariosPorEquipo(int equipoId);
        List<UsuarioViewModel> GetUsuariosNotInEquipo(int equipoId);
        EquipoViewModel GetEquipoPorUsuario(int usuarioId);
    }
}