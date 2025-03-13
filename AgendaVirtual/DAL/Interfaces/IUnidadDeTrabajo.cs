using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IUnidadDeTrabajo : IDisposable
    {
        IUsuarioDAL usuarioDAL { get; set; }
        IEquipoDAL equipoDAL { get; set; }
        IUsuarioEquipoDAL usuarioEquipoDAL { get; set; }
        bool Complete();
    }
}
