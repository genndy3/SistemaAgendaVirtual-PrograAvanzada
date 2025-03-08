using System;
using System.Collections.Generic;

namespace Entities.Entities;

public partial class Evento
{
    public int IdEvento { get; set; }

    public int IdUsuario { get; set; }

    public int? IdEquipo { get; set; }

    public string Titulo { get; set; } = null!;

    public string? Descripcion { get; set; }

    public DateTime FechaHoraInicio { get; set; }

    public DateTime FechaHoraFin { get; set; }

    public string? Ubicacion { get; set; }

    public virtual Equipo? IdEquipoNavigation { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
