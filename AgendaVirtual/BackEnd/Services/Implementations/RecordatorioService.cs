using BackEnd.DTO;
using BackEnd.Services.Interfaces;
using DAL.Implementations;
using DAL.Interfaces;
using Entities.Entities;

namespace BackEnd.Services.Implementations
{
    public class RecordatorioService : IRecordatorioService
    {
        IUnidadDeTrabajo _unidadDeTrabajo;

        public RecordatorioService(IUnidadDeTrabajo unidadDeTrabajo)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
        }

        RecordatorioDTO Convertir(Recordatorio recordatorio)
        {
            return new RecordatorioDTO
            {
                IdRecordatorio = recordatorio.IdRecordatorio,
                IdUsuario = recordatorio.IdUsuario,
                IdTarea = recordatorio.IdTarea,
                Mensaje = recordatorio.Mensaje,
                FechaHora = recordatorio.FechaHora
            };
        }
        Recordatorio Convertir(RecordatorioDTO recordatorioDTO)
        {
            return new Recordatorio
            {
                IdRecordatorio = recordatorioDTO.IdRecordatorio,
                IdUsuario = recordatorioDTO.IdUsuario,
                IdTarea = recordatorioDTO.IdTarea,
                Mensaje = recordatorioDTO.Mensaje,
                FechaHora = recordatorioDTO.FechaHora
            };
        }
        public RecordatorioDTO Add(RecordatorioDTO recordatorioDTO)
        {
            _unidadDeTrabajo.recordatorioDAL.Add(Convertir(recordatorioDTO));
            _unidadDeTrabajo.Complete();
            return recordatorioDTO;
        }

        public RecordatorioDTO Delete(int id)
        {
            Recordatorio recordatorio = new Recordatorio { IdRecordatorio = id };
            _unidadDeTrabajo.recordatorioDAL.Remove(recordatorio);
            _unidadDeTrabajo.Complete();
            return Convertir(recordatorio);
        }

        public RecordatorioDTO Get(int id)
        {
            var recordatorio = _unidadDeTrabajo.recordatorioDAL.Get(id);
            return Convertir(recordatorio);
        }

        public List<RecordatorioDTO> GetAll()
        {
            var recordatorios = _unidadDeTrabajo.recordatorioDAL.GetAll();
            List<RecordatorioDTO> recordatoriosDTO = new List<RecordatorioDTO>();
            foreach (var recordatorio in recordatorios)
            {
                recordatoriosDTO.Add(Convertir(recordatorio));
            }
            return recordatoriosDTO;
        }

        public RecordatorioDTO Update(RecordatorioDTO recordatorioDTO)
        {
            _unidadDeTrabajo.recordatorioDAL.Update(Convertir(recordatorioDTO));
            _unidadDeTrabajo.Complete();
            return recordatorioDTO;
        }

        public List<RecordatorioDTO> getAllByTarea(int idTarea)
        {
            var recordatorios = _unidadDeTrabajo.recordatorioDAL.getRecordatoriosByTarea(idTarea);
            List<RecordatorioDTO> recordatoriosDTO = new List<RecordatorioDTO>();
            foreach (var recordatorio in recordatorios)
            {
                recordatoriosDTO.Add(Convertir(recordatorio));
            }
            return recordatoriosDTO;
        }

        public List<RecordatorioDTO> getAllNotInTarea(int idTarea)
        {
            var recordatorios = _unidadDeTrabajo.recordatorioDAL.getRecordatoriosNotInTarea(idTarea);
            List<RecordatorioDTO> recordatoriosDTO = new List<RecordatorioDTO>();
            foreach (var recordatorio in recordatorios)
            {
                recordatoriosDTO.Add(Convertir(recordatorio));
            }
            return recordatoriosDTO;
        }
    }
}
