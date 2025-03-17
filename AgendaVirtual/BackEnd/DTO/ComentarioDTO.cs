namespace BackEnd.DTO
{
    public class ComentarioDTO
    {
        public int IdComentario { get; set; }

        public int IdTarea { get; set; }

        public int IdUsuario { get; set; }

        public string Texto { get; set; } = null!;

        public DateTime? FechaHora { get; set; }
    }
}
