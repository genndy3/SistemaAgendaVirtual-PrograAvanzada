using BackEnd.DTO;

namespace BackEnd.Services.Interfaces
{
    public interface IComentarioService
    {
        ComentarioDTO Get(int id);
        List<ComentarioDTO> GetAll();
        ComentarioDTO Add(ComentarioDTO comentarioDTO);
        ComentarioDTO Update(ComentarioDTO comentarioDTO);
        ComentarioDTO Delete(int id);
        List<ComentarioDTO> GetAllByTarea(int idTarea);
    }
}
