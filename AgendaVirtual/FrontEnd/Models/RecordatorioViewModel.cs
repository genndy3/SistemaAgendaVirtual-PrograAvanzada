namespace FrontEnd.Models
{
    public class RecordatorioViewModel
    {
        public int IdRecordatorio { get; set; }

        public int IdUsuario { get; set; }

        public int? IdTarea { get; set; }

        public string Mensaje { get; set; } = null!;

        public DateTime FechaHora { get; set; }
        public string? TituloTarea { get; set; }
    }
}
