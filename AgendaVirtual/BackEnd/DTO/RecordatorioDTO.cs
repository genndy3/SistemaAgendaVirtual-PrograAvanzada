namespace BackEnd.DTO
{
    public class RecordatorioDTO
    {
        public int IdRecordatorio { get; set; }

        public int IdUsuario { get; set; }

        public int? IdTarea { get; set; }

        public string Mensaje { get; set; } = null!;

        public DateTime FechaHora { get; set; }
    }
}
