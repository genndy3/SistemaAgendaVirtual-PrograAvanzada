using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using DAL.Interfaces;
using Entities.Entities;

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

        public bool Add(Equipo entity)
        {
            try
            {
                string sql = "exec [dbo].[AgregarEquipo] @nombre, @descripcion, @fecha_creacion";
                var param = new SqlParameter[]
                {
                    new SqlParameter()
                    {
                        ParameterName = "@nombre",
                        SqlDbType = System.Data.SqlDbType.NVarChar,
                        Value = entity.Nombre
                    },
                    new SqlParameter()
                    {
                        ParameterName = "@descripcion",
                        SqlDbType = System.Data.SqlDbType.NVarChar,
                        Value = entity.Descripcion
                    },
                    new SqlParameter()
                    {
                        ParameterName = "@fecha_creacion",
                        SqlDbType = System.Data.SqlDbType.DateTime,
                        Value = entity.FechaCreacion
                    }
                };
                _context.Database.ExecuteSqlRaw(sql, param);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}