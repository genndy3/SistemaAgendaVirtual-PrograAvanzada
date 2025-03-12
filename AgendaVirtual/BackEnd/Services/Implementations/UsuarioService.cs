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
            _unidadDeTrabajo.UsuarioDAL.Add(Convertir(usuarioDTO));
            _unidadDeTrabajo.Complete();
            return usuarioDTO;
        }

        public UsuarioDTO Delete(int id)
        {
            Usuario usuario = new Usuario { IdUsuario = id };
            _unidadDeTrabajo.UsuarioDAL.Remove(usuario);
            _unidadDeTrabajo.Complete();
            return Convertir(usuario);
        }

        public List<UsuarioDTO> GetAll()
        {
            var usuarios = _unidadDeTrabajo.UsuarioDAL.GetAll();
            List<UsuarioDTO> employeesDTO = new List<UsuarioDTO>();
            foreach (var usuario in usuarios)
            {
                employeesDTO.Add(Convertir(usuario));
            }
            return employeesDTO;
        }

        public UsuarioDTO Get(int id)
        {
            var employee = _unidadDeTrabajo.UsuarioDAL.Get(id);
            return Convertir(employee);
        }

        public UsuarioDTO Update(UsuarioDTO usuarioDTO)
        {
            _unidadDeTrabajo.UsuarioDAL.Update(Convertir(usuarioDTO));
            _unidadDeTrabajo.Complete();
            return usuarioDTO;
        }
    }
}
