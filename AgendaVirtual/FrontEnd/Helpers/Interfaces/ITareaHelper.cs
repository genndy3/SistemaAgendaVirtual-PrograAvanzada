using FrontEnd.Models;

namespace FrontEnd.Helpers.Interfaces
{
    public interface ITareaHelper
    {
        string Token { get; set; }

        List<TareaViewModel> GetAll();
        TareaViewModel GetById(int id);
        TareaViewModel AddTarea(TareaViewModel tarea);
        TareaViewModel UpdateTarea(TareaViewModel tarea);
        void DeleteTarea(int id);
        TareaViewModel getTarea(int id);

        List<TareaViewModel> GetTareasPersonales(int usuarioId);
        List<TareaViewModel> GetTareasPorEquipo(int equipoId);
    }
}