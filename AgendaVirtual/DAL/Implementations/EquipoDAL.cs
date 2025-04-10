using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interfaces;
using Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Implementations
{
    public class EquipoDAL : DALGenericoImpl<Equipo>, IEquipoDAL
    {
        private AgendaVirtualContext _context;
        public EquipoDAL(AgendaVirtualContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Equipo> GetAllByUser(int idUsuario)
        {
            return _context.Equipos
                .Where(e => _context.UsuarioEquipos
                    .Any(ue => ue.IdUsuario == idUsuario && ue.IdEquipo == e.IdEquipo))
                .ToList();
        }

        public IEnumerable<Usuario> GetUsuariosByEquipo(int idEquipo)
        {
            return _context.Usuarios
                .Where(u => _context.UsuarioEquipos
                    .Any(ue => ue.IdEquipo == idEquipo && ue.IdUsuario == u.IdUsuario))
                .ToList();
        }

        public IEnumerable<Usuario> GetUsuariosNotInEquipo(int idEquipo)
        {
            return _context.Usuarios
                .Where(u => !_context.UsuarioEquipos
                    .Any(ue => ue.IdEquipo == idEquipo && ue.IdUsuario == u.IdUsuario))
                .ToList();
        }
        public Equipo GetEquipoByUsuario(int idUsuario)
        {
            return _context.Equipos
                .Where(e => _context.UsuarioEquipos
                    .Any(ue => ue.IdUsuario == idUsuario && ue.IdEquipo == e.IdEquipo))
                .FirstOrDefault();
        }

        public Equipo agregarEquipo(Equipo equipo)
        {
            _context.Equipos.Add(equipo);
            _context.SaveChanges();
            return equipo;
        }
    }
}