﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IUnidadDeTrabajo : IDisposable
    {
        IUsuarioDAL usuarioDAL { get; set; }
        IEquipoDAL equipoDAL { get; set; }
        IUsuarioEquipoDAL usuarioEquipoDAL { get; set; }
        ITareaDAL tareaDAL { get; set; }
        IRecordatorioDAL recordatorioDAL { get; set; }
        IEventoDAL eventoDAL { get; set; }
        IComentarioDAL comentarioDAL { get; set; }
        bool Complete();
    }
}
