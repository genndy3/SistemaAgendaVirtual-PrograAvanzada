using System;
using System.Collections.Generic;

namespace Entities.Entities;

public partial class Recordatorio
{
    public int IdRecordatorio { get; set; }

    public int IdUsuario { get; set; }

    public int? IdTarea { get; set; }

    public string Mensaje { get; set; } = null!;

    public DateTime FechaHora { get; set; }

    public bool? Enviado { get; set; }

    public virtual Tarea? IdTareaNavigation { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
