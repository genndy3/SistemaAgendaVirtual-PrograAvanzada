﻿namespace FrontEnd.Models
{
    public class EquipoViewModel
    {
        public int IdEquipo { get; set; }

        public string Nombre { get; set; } = null!;

        public string? Descripcion { get; set; }

        public DateTime? FechaCreacion { get; set; }
    }
}
