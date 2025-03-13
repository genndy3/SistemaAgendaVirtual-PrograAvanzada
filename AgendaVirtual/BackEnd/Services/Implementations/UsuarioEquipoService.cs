using BackEnd.DTO;
using BackEnd.Services.Interfaces;
using DAL.Implementations;
using DAL.Interfaces;
using Entities.Entities;

namespace BackEnd.Services.Implementations
{
    public class UsuarioEquipoService : IUsuarioEquipoService
    {
        IUnidadDeTrabajo _unidadDeTrabajo;

        public UsuarioEquipoService(IUnidadDeTrabajo unidadDeTrabajo)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
        }
        UsuarioEquipoDTO Convertir(UsuarioEquipo usuarioEquipo)
        {
            return new UsuarioEquipoDTO
            {
                IdUsuario = usuarioEquipo.IdUsuario,
                IdEquipo = usuarioEquipo.IdEquipo,
                FechaAsignacion = usuarioEquipo.FechaAsignacion
            };
        }

        UsuarioEquipo Convertir(UsuarioEquipoDTO usuarioEquipoDTO)
        {
            return new UsuarioEquipo
            {
                IdUsuario = usuarioEquipoDTO.IdUsuario,
                IdEquipo = usuarioEquipoDTO.IdEquipo,
                FechaAsignacion = usuarioEquipoDTO.FechaAsignacion
            };
        }

        public UsuarioEquipoDTO Add(UsuarioEquipoDTO usuarioEquipoDTO)
        {
            _unidadDeTrabajo.usuarioEquipoDAL.Add(Convertir(usuarioEquipoDTO));
            _unidadDeTrabajo.Complete();
            return usuarioEquipoDTO;
        }

        public UsuarioEquipoDTO Delete(int idUsuario, int idEquipo)
        {
            UsuarioEquipo usuarioEquipo = new UsuarioEquipo { IdUsuario = idUsuario, IdEquipo = idEquipo };
            _unidadDeTrabajo.usuarioEquipoDAL.Remove(usuarioEquipo);
            _unidadDeTrabajo.Complete();
            return Convertir(usuarioEquipo);
        }

        public UsuarioEquipoDTO Get(int idUsuario, int idEquipo)
        {
            var usuario = _unidadDeTrabajo.usuarioEquipoDAL.Get(idUsuario, idEquipo);
            return Convertir(usuario);
        }

        public List<UsuarioEquipoDTO> GetAll()
        {
            var usuarioEquipos = _unidadDeTrabajo.usuarioEquipoDAL.GetAll();
            List<UsuarioEquipoDTO> usuarioEquiposDTO = new List<UsuarioEquipoDTO>();
            foreach (var usuarioEquipo in usuarioEquipos)
            {
                usuarioEquiposDTO.Add(Convertir(usuarioEquipo));
            }
            return usuarioEquiposDTO;
        }

        public UsuarioEquipoDTO Update(UsuarioEquipoDTO usuarioEquipoDTO)
        {
            _unidadDeTrabajo.usuarioEquipoDAL.Update(Convertir(usuarioEquipoDTO));
            _unidadDeTrabajo.Complete();
            return usuarioEquipoDTO;
        }
    }
}
