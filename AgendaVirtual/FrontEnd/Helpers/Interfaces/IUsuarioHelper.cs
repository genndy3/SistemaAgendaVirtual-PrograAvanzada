using FrontEnd.Models;

namespace FrontEnd.Helpers.Interfaces
{
    public interface IUsuarioHelper
    {
        string Token { get; set; }
        List<UsuarioViewModel> getUsuarios();
        UsuarioViewModel getUsuario(int id);
        UsuarioViewModel addUsuario(UsuarioViewModel usuario);
        UsuarioViewModel updateUsuario(UsuarioViewModel usuario);
        void deleteUsuario(int id);
    }
}