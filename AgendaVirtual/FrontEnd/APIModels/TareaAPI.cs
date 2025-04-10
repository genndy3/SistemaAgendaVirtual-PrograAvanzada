namespace FrontEnd.APIModels
{
    public class TareaAPI
    {
        public int IdTarea { get; set; }

        public int IdUsuario { get; set; }

        public int? IdEquipo { get; set; }

        public string Titulo { get; set; } = null!;

        public string? Descripcion { get; set; }

        public DateTime? FechaLimite { get; set; }

        public string Prioridad { get; set; } = null!;

        public string Estado { get; set; } = null!;
    }
}