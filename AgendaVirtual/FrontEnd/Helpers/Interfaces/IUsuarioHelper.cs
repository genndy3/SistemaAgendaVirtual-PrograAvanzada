using FrontEnd.Models;

namespace FrontEnd.Helpers.Interfaces
{
    public interface IUsuarioHelper
    {
        string Token { get; set; }  
        
        List<UsuarioViewModel> GetUsuarios();  
        UsuarioViewModel GetUsuario(int id);  
        UsuarioViewModel AddUsuario(UsuarioViewModel usuario); 
        UsuarioViewModel UpdateUsuario(UsuarioViewModel usuario);  
        void DeleteUsuario(int id);  
    }
}