using BackEnd.DTO;

namespace BackEnd.Services.Interfaces
{
    public interface IEventoService
    {
        EventoDTO Get(int id);
        List<EventoDTO> GetAll();
        EventoDTO Add(EventoDTO eventoDTO);
        EventoDTO Update(EventoDTO eventoDTO);
        EventoDTO Delete(int id);

    }
}
