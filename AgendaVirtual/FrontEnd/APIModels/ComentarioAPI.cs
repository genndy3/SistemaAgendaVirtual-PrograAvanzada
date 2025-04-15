namespace FrontEnd.APIModels
{
    public class ComentarioAPI
    {
        public int IdComentario { get; set; }

        public int IdTarea { get; set; }

        public int IdUsuario { get; set; }

        public string Texto { get; set; } = null!;

        public DateTime? FechaHora { get; set; }
    }
}
