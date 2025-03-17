using BackEnd.DTO;
using BackEnd.Services.Interfaces;
using DAL.Interfaces;
using Entities.Entities;

namespace BackEnd.Services.Implementations
{
    public class EventoService : IEventoService
    {
        IUnidadDeTrabajo _unidadDeTrabajo;

        public EventoService(IUnidadDeTrabajo unidadDeTrabajo)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
        }

        EventoDTO Convertir(Evento evento)
        {
            return new EventoDTO
            {
                IdEvento = evento.IdEvento,
                IdUsuario = evento.IdUsuario,
                IdEquipo = evento.IdEquipo,
                Titulo = evento.Titulo,
                Descripcion = evento.Descripcion,
                FechaHoraInicio = evento.FechaHoraInicio,
                FechaHoraFin = evento.FechaHoraFin,
                Ubicacion = evento.Ubicacion
            };
        }

        Evento Convertir(EventoDTO eventoDTO)
        {
            return new Evento
            {
                IdEvento = eventoDTO.IdEvento,
                IdUsuario = eventoDTO.IdUsuario,
                IdEquipo = eventoDTO.IdEquipo,
                Titulo = eventoDTO.Titulo,
                Descripcion = eventoDTO.Descripcion,
                FechaHoraInicio = eventoDTO.FechaHoraInicio,
                FechaHoraFin = eventoDTO.FechaHoraFin,
                Ubicacion = eventoDTO.Ubicacion
            };
        }

        public EventoDTO Add(EventoDTO eventoDTO)
        {
            _unidadDeTrabajo.eventoDAL.Add(Convertir(eventoDTO));
            _unidadDeTrabajo.Complete();
            return eventoDTO;
        }

        public EventoDTO Delete(int id)
        {
           Evento evento = new Evento { IdEvento = id };
            _unidadDeTrabajo.eventoDAL.Remove(evento);
            _unidadDeTrabajo.Complete();
            return Convertir(evento);
        }

        public EventoDTO Get(int id)
        {
            var evento = _unidadDeTrabajo.eventoDAL.Get(id);
            return Convertir(evento);
        }

        public List<EventoDTO> GetAll()
        {
            var eventos = _unidadDeTrabajo.eventoDAL.GetAll();
            List<EventoDTO> eventosDTO = new List<EventoDTO>();
            foreach (var evento in eventos)
            {
                eventosDTO.Add(Convertir(evento));
            }
            return eventosDTO;
        }

        public EventoDTO Update(EventoDTO eventoDTO)
        {
            _unidadDeTrabajo.eventoDAL.Update(Convertir(eventoDTO));
            _unidadDeTrabajo.Complete();
            return eventoDTO;
        }
    }
}
