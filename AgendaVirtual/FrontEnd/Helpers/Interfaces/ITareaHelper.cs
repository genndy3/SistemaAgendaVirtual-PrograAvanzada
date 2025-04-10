using FrontEnd.Models;

namespace FrontEnd.Helpers.Interfaces
{
    public interface ITareaHelper
    {

        List<TareaViewModel> GetAll();
        TareaViewModel GetById(int id);
        TareaViewModel AddTarea(TareaViewModel tareaViewModel);
        TareaViewModel UpdateTarea(TareaViewModel tareaViewModel);
        void DeleteTarea(int id);
    }
}
