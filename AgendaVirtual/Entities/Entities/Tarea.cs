using System;
using System.Collections.Generic;

namespace Entities.Entities;

public partial class Tarea
{
    public int IdTarea { get; set; }

    public int IdUsuario { get; set; }

    public int? IdEquipo { get; set; }

    public string Titulo { get; set; } = null!;

    public string? Descripcion { get; set; }

    public DateTime? FechaLimite { get; set; }

    public string Prioridad { get; set; } = null!;

    public string Estado { get; set; } = null!;

    public virtual ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();

    public virtual Equipo? IdEquipoNavigation { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

    public virtual ICollection<Recordatorio> Recordatorios { get; set; } = new List<Recordatorio>();
}
