using FrontEnd.Models;

namespace FrontEnd.Helpers.Interfaces
{
    public interface IUsuarioEquipoHelper
    {
        string Token { get; set; }
        UsuarioEquipoViewModel addUsuarioEquipo(UsuarioEquipoViewModel tarea);
        void deleteUsuarioEquipo(int idUsuario, int idEquipo);
    }
}