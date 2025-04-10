using FrontEnd.Models;

namespace FrontEnd.Helpers.Interfaces
{
    public interface ITareaHelper
    {
        string Token { get; set; }
        List<TareaViewModel> getTareas();
        List<TareaViewModel> getTareasPersonales(int usuarioId);
        List<TareaViewModel> getTareasPorEquipo(int usuarioId);
        TareaViewModel getTarea(int id);
        TareaViewModel addTarea(TareaViewModel tarea);
        TareaViewModel updateTarea(TareaViewModel tarea);
        void deleteTarea(int id);
    }
}