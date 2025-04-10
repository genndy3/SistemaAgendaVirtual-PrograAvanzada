using BackEnd.DTO;
using BackEnd.Services.Interfaces;
using DAL.Interfaces;
using Entities.Entities;

namespace BackEnd.Services.Implementations
{
    public class EquipoService : IEquipoService
    {
        IUnidadDeTrabajo _unidadDeTrabajo;

        public EquipoService(IUnidadDeTrabajo unidadDeTrabajo)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
        }

        EquipoDTO Convertir(Equipo equipo)
        {
            return new EquipoDTO
            {
                IdEquipo = equipo.IdEquipo,
                Nombre = equipo.Nombre,
                Descripcion = equipo.Descripcion,
                FechaCreacion = equipo.FechaCreacion
            };
        }

        Equipo Convertir(EquipoDTO equipoDTO)
        {
            return new Equipo
            {
                IdEquipo = equipoDTO.IdEquipo,
                Nombre = equipoDTO.Nombre,
                Descripcion = equipoDTO.Descripcion,
                FechaCreacion = equipoDTO.FechaCreacion
            };
        }

        UsuarioDTO convertirUsuario(Usuario usuario)
        {
            return new UsuarioDTO
            {
                IdUsuario = usuario.IdUsuario,
                Nombre = usuario.Nombre,
                Correo = usuario.Correo,
                FechaRegistro = usuario.FechaRegistro,
                Rol = usuario.Rol
            };
        }

        public EquipoDTO Add(EquipoDTO equipoDTO)
        {
            Equipo nuevoEquipo = _unidadDeTrabajo.equipoDAL.agregarEquipo(Convertir(equipoDTO));
            _unidadDeTrabajo.Complete();
            return Convertir(nuevoEquipo);
        }

        public void AddEquipo(EquipoDTO equipo)
        {
            var categoryEntity = Convertir(equipo);
            _unidadDeTrabajo.equipoDAL.Add(categoryEntity);
            _unidadDeTrabajo.Complete();
        }

        public EquipoDTO Delete(int id)
        {
            Equipo equipo = new Equipo { IdEquipo = id };
            _unidadDeTrabajo.equipoDAL.Remove(equipo);
            _unidadDeTrabajo.Complete();
            return Convertir(equipo);
        }

        public EquipoDTO Get(int id)
        {
            var equipo = _unidadDeTrabajo.equipoDAL.Get(id);
            return Convertir(equipo);
        }

        public List<EquipoDTO> GetEquipo()
        {
            var equipos = _unidadDeTrabajo.equipoDAL.GetAll();
            List<EquipoDTO> equiposDTO = new List<EquipoDTO>();
            foreach (var equipo in equipos)
            {
                equiposDTO.Add(Convertir(equipo));
            }
            return equiposDTO;
        }

        public EquipoDTO Update(EquipoDTO equipoDTO)
        {
            _unidadDeTrabajo.equipoDAL.Update(Convertir(equipoDTO));
            _unidadDeTrabajo.Complete();
            return equipoDTO;
        }

        public List<EquipoDTO> GetAllByUser(int idUsuario)
        {
            var equipos = _unidadDeTrabajo.equipoDAL.GetAllByUser(idUsuario);
            List<EquipoDTO> equiposDTO = new List<EquipoDTO>();

            foreach (var equipo in equipos)
            {
                equiposDTO.Add(Convertir(equipo));
            }

            return equiposDTO;
        }

        public List<UsuarioDTO> GetUsuariosByEquipo(int idEquipo)
        {
            var usuarios = _unidadDeTrabajo.equipoDAL.GetUsuariosByEquipo(idEquipo);
            List<UsuarioDTO> usuariosDTO = new List<UsuarioDTO>();

            foreach (var usuario in usuarios)
            {
                usuariosDTO.Add(convertirUsuario(usuario));
            }

            return usuariosDTO;
        }

        public List<UsuarioDTO> GetUsuariosNotIntEquipo(int idEquipo)
        {
            var usuarios = _unidadDeTrabajo.equipoDAL.GetUsuariosNotInEquipo(idEquipo);
            List<UsuarioDTO> usuariosDTO = new List<UsuarioDTO>();
            foreach (var usuario in usuarios)
            {
                usuariosDTO.Add(convertirUsuario(usuario));
            }
            return usuariosDTO;
        }

        public EquipoDTO GetEquipoPorUsuario(int idUsuario)
        {
            var equipo = _unidadDeTrabajo.equipoDAL.GetEquipoByUsuario(idUsuario);
            return Convertir(equipo);
        }
    }
}
