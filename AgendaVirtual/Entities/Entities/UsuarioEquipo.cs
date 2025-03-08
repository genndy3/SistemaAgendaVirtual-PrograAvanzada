using System;
using System.Collections.Generic;

namespace Entities.Entities;

public partial class UsuarioEquipo
{
    public int IdUsuario { get; set; }

    public int IdEquipo { get; set; }

    public DateTime? FechaAsignacion { get; set; }

    public virtual Equipo IdEquipoNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
