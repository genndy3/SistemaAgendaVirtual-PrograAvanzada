using FrontEnd.Models;

namespace FrontEnd.Helpers.Interfaces
{
    public interface ITareaHelper
    {
        string Token { get; set; }

        List<TareaViewModel> GetAll();
        TareaViewModel GetById(int id);
        TareaViewModel AddTarea(TareaViewModel tareaViewModel);
        TareaViewModel UpdateTarea(TareaViewModel tareaViewModel);
        void DeleteTarea(int id);

        List<TareaViewModel> GetTareasPersonales(int usuarioId);
        List<TareaViewModel> GetTareasPorEquipo(int equipoId);
        TareaViewModel GetTarea(int id);
    }
}
