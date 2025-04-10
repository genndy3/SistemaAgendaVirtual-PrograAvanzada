using BackEnd.DTO;
using BackEnd.Services.Interfaces;
using DAL.Implementations;
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

    }
}
