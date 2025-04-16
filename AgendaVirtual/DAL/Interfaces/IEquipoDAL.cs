using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Entities;

namespace DAL.Interfaces
{
    public interface IEquipoDAL : IDALGenerico<Equipo>
    {
        IEnumerable<Equipo> GetAllByUser(int idUsuario);
        IEnumerable<Usuario> GetUsuariosByEquipo(int idEquipo);
        IEnumerable<Usuario> GetUsuariosNotInEquipo(int idEquipo);
        Equipo GetEquipoByUsuario(int idUsuario);
        Equipo agregarEquipo(Equipo equipo);
    }
}
