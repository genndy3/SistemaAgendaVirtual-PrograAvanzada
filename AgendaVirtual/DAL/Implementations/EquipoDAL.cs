using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interfaces;
using Entities.Entities;
using Microsoft.Data.SqlClient;
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
                _context
                    .Database
                    .ExecuteSqlRaw(sql, param);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }
    }
}
