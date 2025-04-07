namespace FrontEnd.Models
{
    public class TareaViewModel
    {
        public int IdTarea { get; set; }

        public int IdUsuario { get; set; }
        public IEnumerable<UsuarioViewModel> Usuarios { get; set; } 
        public int? IdEquipo { get; set; }
        public IEnumerable<EquipoViewModel> Equipos { get; set; }
        public string Titulo { get; set; } = null!;

        public string? Descripcion { get; set; }

        public DateTime? FechaLimite { get; set; }

        public string Prioridad { get; set; } = null!;

        public string Estado { get; set; } = null!;

    }
}
