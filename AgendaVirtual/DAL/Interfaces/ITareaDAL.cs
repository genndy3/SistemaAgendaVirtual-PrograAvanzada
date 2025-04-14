using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface ITareaDAL : IDALGenerico<Tarea>
    {
        public IEnumerable<Tarea> getAllByUser(int idUsuario);
        public IEnumerable<Tarea> getAllByEquipoAndUser(int idUsuario);
        public Tarea AgregarTarea(Tarea tarea);
    }
}
