using System.Diagnostics;
using BackEnd.DTO;
using BackEnd.Services.Interfaces;
using DAL.Interfaces;
using Entities.Entities;

namespace BackEnd.Services.Implementations
{
    public class UsuarioService : IUsuarioService
    {
        IUnidadDeTrabajo _unidadDeTrabajo;
        public UsuarioService(IUnidadDeTrabajo unidadDeTrabajo)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
        }
        UsuarioDTO Convertir(Usuario usuario)
        {
            return new UsuarioDTO
            {
                IdUsuario = usuario.IdUsuario,
                Contrasena = usuario.Contrasena,
                Correo = usuario.Correo,
                Nombre = usuario.Nombre,
                Rol = usuario.Rol,
                FechaRegistro = usuario.FechaRegistro
            };
        }
        Usuario Convertir(UsuarioDTO usuarioDTO)
        {
            return new Usuario
            {
                IdUsuario = usuarioDTO.IdUsuario,
                Contrasena = usuarioDTO.Contrasena,
                Correo = usuarioDTO.Correo,
                Nombre = usuarioDTO.Nombre,
                Rol = usuarioDTO.Rol,
                FechaRegistro = usuarioDTO.FechaRegistro
            };
        }

        public UsuarioDTO Add(UsuarioDTO usuarioDTO)
        {
            _unidadDeTrabajo.usuarioDAL.Add(Convertir(usuarioDTO));
            _unidadDeTrabajo.Complete();
            return usuarioDTO;
        }

        public UsuarioDTO Delete(int id)
        {
            Usuario usuario = new Usuario { IdUsuario = id };
            try
            {
                bool deleteSuccess = _unidadDeTrabajo.usuarioDAL.DeleteSP(usuario);
                if (!deleteSuccess)
                {
                    throw new Exception("No se pudo eliminar el usuario con el SP.");
                }

                _unidadDeTrabajo.usuarioDAL.Remove(usuario);
                _unidadDeTrabajo.Complete();

                return Convertir(usuario);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al eliminar usuario: {ex.Message}");
                throw;
            }
        }

        public List<UsuarioDTO> GetAll()
        {
            var usuarios = _unidadDeTrabajo.usuarioDAL.GetAll();
            List<UsuarioDTO> usuariosDTO = new List<UsuarioDTO>();
            foreach (var usuario in usuarios)
            {
                usuariosDTO.Add(Convertir(usuario));
            }
            return usuariosDTO;
        }

        public UsuarioDTO Get(int id)
        {
            var usuario = _unidadDeTrabajo.usuarioDAL.Get(id);
            return Convertir(usuario);
        }

        public UsuarioDTO Update(UsuarioDTO usuarioDTO)
        {
            _unidadDeTrabajo.usuarioDAL.Update(Convertir(usuarioDTO));
            _unidadDeTrabajo.Complete();
            _unidadDeTrabajo.usuarioDAL.UpdateSP(Convertir(usuarioDTO));
            return usuarioDTO;
        }
    }
}
