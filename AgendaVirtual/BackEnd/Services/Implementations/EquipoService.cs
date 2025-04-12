using BackEnd.DTO;
using BackEnd.Services.Interfaces;
using DAL.Implementations;
using DAL.Interfaces;
using Entities.Entities;
using Microsoft.EntityFrameworkCore;

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

        UsuarioDTO ConvertirUsuario(Usuario usuario)
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

        // Método Add (devuelve DTO, de la rama feature)
        public EquipoDTO Add(EquipoDTO equipoDTO)
        {
            Equipo nuevoEquipo = _unidadDeTrabajo.equipoDAL.Add(Convertir(equipoDTO));
            _unidadDeTrabajo.Complete();
            return Convertir(nuevoEquipo);
        }

        // Método alternativo (si se necesita void, de la rama dev)
        public void AddEquipo(EquipoDTO equipo)
        {
            var equipoEntity = Convertir(equipo);
            _unidadDeTrabajo.equipoDAL.Add(equipoEntity);
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

        public List<EquipoDTO> GetAll()
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

        // Métodos adicionales de la rama feature/equipos-(frontend)
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
                usuariosDTO.Add(ConvertirUsuario(usuario));
            }

            return usuariosDTO;
        }

        public List<UsuarioDTO> GetUsuariosNotInEquipo(int idEquipo)
        {
            var usuarios = _unidadDeTrabajo.equipoDAL.GetUsuariosNotInEquipo(idEquipo);
            List<UsuarioDTO> usuariosDTO = new List<UsuarioDTO>();
            foreach (var usuario in usuarios)
            {
                usuariosDTO.Add(ConvertirUsuario(usuario));
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