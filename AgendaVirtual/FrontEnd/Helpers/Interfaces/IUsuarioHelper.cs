using FrontEnd.Models;

namespace FrontEnd.Helpers.Interfaces
{
    public interface IUsuarioHelper
    {
        List<UsuarioViewModel> GetAll();
        UsuarioViewModel GetById(int id);
        UsuarioViewModel AddUsuario(UsuarioViewModel UsuarioViewModel);
        UsuarioViewModel UpdateUsuario(UsuarioViewModel UsuarioViewModel);
        void DeleteUsuario(int id);
    }
}
