using FrontEnd.Models;

namespace FrontEnd.Helpers.Interfaces
{
    public interface IEquipoHelper
    {
        List<EquipoViewModel> GetEquipos();
        EquipoViewModel GetById(int id);
        EquipoViewModel Add(EquipoViewModel equipo);
        EquipoViewModel EditEquipo(EquipoViewModel EquipoViewModel);
        void DeleteEquipo(int id);
    }
}
