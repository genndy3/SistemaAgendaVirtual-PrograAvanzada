using FrontEnd.Models;

namespace FrontEnd.Helpers.Interfaces
{
    public interface IUsuarioHelper
    {
        string Token { get; set; }  
        
        List<UsuarioViewModel> GetAll();  
        UsuarioViewModel GetById(int id);  
        UsuarioViewModel AddUsuario(UsuarioViewModel usuario); 
        UsuarioViewModel UpdateUsuario(UsuarioViewModel usuario);  
        void DeleteUsuario(int id);  
    }
}