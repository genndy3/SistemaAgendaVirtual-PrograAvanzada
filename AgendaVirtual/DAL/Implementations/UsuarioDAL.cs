using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DAL.Interfaces;
using Entities.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DAL.Implementations
{
    public class UsuarioDAL : DALGenericoImpl<Usuario>, IUsuarioDAL
    {
        private AgendaVirtualContext _context;

        public UsuarioDAL(AgendaVirtualContext context) : base(context)
        {
            _context = context;
        }

        public bool UpdateSP(Usuario usuario)
        {
            try
            {
                string sql = "exec [dbo].[UpdateUsuario] @id_usuario, @nombre, @correo, @rol";

                var param = new SqlParameter[]
                {
                    new SqlParameter("@id_usuario", System.Data.SqlDbType.NVarChar) { Value = usuario.IdUsuario },
                    new SqlParameter("@nombre", System.Data.SqlDbType.NVarChar) { Value = usuario.Nombre },
                    new SqlParameter("@correo", System.Data.SqlDbType.NVarChar) { Value = usuario.Correo },
                    new SqlParameter("@rol", System.Data.SqlDbType.NVarChar) { Value = usuario.Rol }
                };

                _context.Database.ExecuteSqlRaw(sql, param);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteSP(Usuario usuario)
        {
            try
            {
                string sql = "exec [dbo].[DeleteUsuario] @id_usuario";

                var param = new SqlParameter[]
                {
                    new SqlParameter("@id_usuario", System.Data.SqlDbType.NVarChar) { Value = usuario.IdUsuario }
                };

                _context.Database.ExecuteSqlRaw(sql, param);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al ejecutar el SP: {ex.Message}");
                return false;
            }
        }

        public int GetIdUser(string id)
        {
            try
            {
                var usuario = _context.Usuarios.FirstOrDefault(u => u.IdIdentity == id);
                return usuario?.IdUsuario ?? 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al ejecutar la consulta: {ex.Message}");
                return 0;
            }
        }
    }
}
