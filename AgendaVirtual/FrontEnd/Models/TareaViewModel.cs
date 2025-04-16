namespace FrontEnd.Models
{
    public class TareaViewModel
    {
        public int IdTarea { get; set; }

        public int IdUsuario { get; set; }

        public int? IdEquipo { get; set; }

        public string Titulo { get; set; } = null!;

        public string? Descripcion { get; set; }

        public DateTime? FechaLimite { get; set; }

        public string Prioridad { get; set; } = null!;

        public string Estado { get; set; } = null!;

        public List<RecordatorioViewModel>? RecordatoriosList { get; set; }

        public string RecordatorioId { get; set; } = null!;

        public string? RecordatorioMensaje{ get; set; }

        public List<ComentarioViewModel>? ComentariosList { get; set; }
        public string ComentarioId { get; set; } = null!;
        public string? ComentarioTexto { get; set; }
        public string? comentarioIdUsuario { get; set; }
    }
}