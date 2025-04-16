using BackEnd.DTO;
using BackEnd.Services.Interfaces;
using DAL.Implementations;
using DAL.Interfaces;
using Entities.Entities;
using System.Threading;

namespace BackEnd.Services.Implementations
{
    public class TareaService : ITareaService
    {
        IUnidadDeTrabajo _unidadDeTrabajo;

        public TareaService(IUnidadDeTrabajo unidadDeTrabajo)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
        }

        TareaDTO Convertir(Tarea tarea)
        {
            return new TareaDTO
            {
                IdTarea = tarea.IdTarea,
                IdUsuario = tarea.IdUsuario,
                IdEquipo = tarea.IdEquipo,
                Titulo = tarea.Titulo,
                Descripcion = tarea.Descripcion,
                FechaLimite = tarea.FechaLimite,
                Prioridad = tarea.Prioridad,
                Estado = tarea.Estado
            };
        }
        Tarea Convertir(TareaDTO tareaDTO)
        {
            return new Tarea
            {
                IdTarea = tareaDTO.IdTarea,
                IdUsuario = tareaDTO.IdUsuario,
                IdEquipo = tareaDTO.IdEquipo,
                Titulo = tareaDTO.Titulo,
                Descripcion = tareaDTO.Descripcion,
                FechaLimite = tareaDTO.FechaLimite,
                Prioridad = tareaDTO.Prioridad,
                Estado = tareaDTO.Estado
            };
        }

        public TareaDTO Add(TareaDTO tareaDTO)
        {
            Tarea nuevaTarea = _unidadDeTrabajo.tareaDAL.AgregarTarea(Convertir(tareaDTO));
            _unidadDeTrabajo.Complete();
            return Convertir(nuevaTarea);
        }

        public TareaDTO Delete(int id)
        {
            Tarea tarea = new Tarea { IdTarea = id };
            _unidadDeTrabajo.tareaDAL.Remove(tarea);
            _unidadDeTrabajo.Complete();
            return Convertir(tarea);
        }

        public TareaDTO Get(int id)
        {
            var tarea = _unidadDeTrabajo.tareaDAL.Get(id);
            return Convertir(tarea);
        }

        public List<TareaDTO> GetAll()
        {
            var tareas = _unidadDeTrabajo.tareaDAL.GetAll();
            List<TareaDTO> tareasDTO = new List<TareaDTO>();
            foreach (var tarea in tareas)
            {
                tareasDTO.Add(Convertir(tarea));
            }
            return tareasDTO;
        }

        public TareaDTO Update(TareaDTO tareaDTO)
        {
            _unidadDeTrabajo.tareaDAL.Update(Convertir(tareaDTO));
            _unidadDeTrabajo.Complete();
            return tareaDTO;
        }

        public List<TareaDTO> GetAllByUser(int idUsuario)
        {
            var tareas = _unidadDeTrabajo.tareaDAL.getAllByUser(idUsuario);
            List<TareaDTO> tareasDTO = new List<TareaDTO>();
            foreach (var tarea in tareas)
            {
                tareasDTO.Add(Convertir(tarea));
            }
            return tareasDTO;
        }

        public List<TareaDTO> GetAllByEquipoAndUser(int idUsuario)
        {
            var tareas = _unidadDeTrabajo.tareaDAL.getAllByEquipoAndUser(idUsuario);
            List<TareaDTO> tareasDTO = new List<TareaDTO>();
            foreach (var tarea in tareas)
            {
                tareasDTO.Add(Convertir(tarea));
            }
            return tareasDTO;
        }
    }
}
