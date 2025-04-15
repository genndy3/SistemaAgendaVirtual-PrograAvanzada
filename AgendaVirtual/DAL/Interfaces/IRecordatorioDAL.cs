using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IRecordatorioDAL : IDALGenerico<Recordatorio>
    {
        Recordatorio addRecordatorio(Recordatorio recordatorio);
        IEnumerable<Recordatorio> getRecordatoriosNotInTarea(int idTarea);
        IEnumerable<Recordatorio> getRecordatoriosByTarea(int idTarea);
    }
}
