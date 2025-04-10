using BackEnd.DTO;

namespace BackEnd.Services.Interfaces
{
    public interface ITareaService
    {
        TareaDTO Get(int id);
        List<TareaDTO> GetAll();
        TareaDTO Add(TareaDTO tareaDTO);
        TareaDTO Update(TareaDTO tareaDTO);
        TareaDTO Delete(int id); 
        List<TareaDTO> GetAllByUser(int idUsuario);
        List<TareaDTO> GetAllByEquipoAndUser(int idUsuario);
    }
}
