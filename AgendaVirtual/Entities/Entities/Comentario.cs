using System;
using System.Collections.Generic;

namespace Entities.Entities;

public partial class Comentario
{
    public int IdComentario { get; set; }

    public int IdTarea { get; set; }

    public int IdUsuario { get; set; }

    public string Texto { get; set; } = null!;

    public DateTime? FechaHora { get; set; }

    public virtual Tarea IdTareaNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
