using DAL.Interfaces;
using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Implementations
{
    public class TareaDAL : DALGenericoImpl<Tarea>, ITareaDAL
    {
        private AgendaVirtualContext _context;
        public TareaDAL(AgendaVirtualContext context) : base(context)
        {
            _context = context;
        }
        public IEnumerable<Tarea> getAllByUser(int idUsuario)
        {
            return _context.Tareas
                   .Where(t => t.IdUsuario == idUsuario &&
                               !_context.UsuarioEquipos.Any(ue => ue.IdUsuario == idUsuario && ue.IdEquipo == t.IdEquipo))
                   .ToList();
        }
        public IEnumerable<Tarea> getAllByEquipoAndUser(int idUsuario)
        {
 
            var idsEquipos = _context.UsuarioEquipos
                .Where(ue => ue.IdUsuario == idUsuario)
                .Select(ue => ue.IdEquipo)
                .ToList();

            return _context.Tareas
                .Where(t => idsEquipos.Contains((int)t.IdEquipo)) 
                .ToList();
        }

        public Tarea AgregarTarea(Tarea tarea)
        {
            _context.Tareas.Add(tarea);
            _context.SaveChanges();
            return tarea;
        }
    }
}
