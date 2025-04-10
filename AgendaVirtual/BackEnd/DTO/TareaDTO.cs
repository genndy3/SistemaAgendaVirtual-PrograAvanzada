using Entities.Entities;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.DTO
{
    public class TareaDTO
    {
        public int IdTarea { get; set; }
        [Required]
        public int IdUsuario { get; set; }
        [Required]
        public int? IdEquipo { get; set; }
        [Required]
        public string Titulo { get; set; } = null!;

        public string? Descripcion { get; set; }

        public DateTime? FechaLimite { get; set; }

        public string Prioridad { get; set; } = null!;

        public string Estado { get; set; } = null!;
    }
}
