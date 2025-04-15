using FrontEnd.Models;

namespace FrontEnd.Helpers.Interfaces
{
    public interface IComentarioHelper
    {
        string Token { get; set; }
        ComentarioViewModel AddComentario(ComentarioViewModel comentario);
        void DeleteComentario(int id);
        List<ComentarioViewModel> GetComentariosByTarea(int tareaId);
    }
}
