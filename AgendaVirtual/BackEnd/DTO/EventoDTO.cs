namespace BackEnd.DTO
{
    public class EventoDTO
    {
        public int IdEvento { get; set; }

        public int IdUsuario { get; set; }

        public int? IdEquipo { get; set; }

        public string Titulo { get; set; } = null!;

        public string? Descripcion { get; set; }

        public DateTime FechaHoraInicio { get; set; }

        public DateTime FechaHoraFin { get; set; }

        public string? Ubicacion { get; set; }
    }
}
