using FrontEnd.Models;

namespace FrontEnd.Helpers.Interfaces
{
    public interface IUsuarioHelper
    {
        string Token { get; set; }

        List<UsuarioViewModel> GetAll();
        UsuarioViewModel GetById(int id);
        UsuarioViewModel AddUsuario(UsuarioViewModel usuarioViewModel);
        UsuarioViewModel UpdateUsuario(UsuarioViewModel usuarioViewModel);
        void DeleteUsuario(int id);

        List<UsuarioViewModel> getUsuarios();
        UsuarioViewModel getUsuario(int id);
    }
}
