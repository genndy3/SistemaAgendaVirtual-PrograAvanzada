using DAL.Interfaces;
using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Implementations
{
    public class UnidadDeTrabajo : IUnidadDeTrabajo
    {
        public IUsuarioDAL usuarioDAL { get; set; }
        public IEquipoDAL equipoDAL { get; set; }
        public IUsuarioEquipoDAL usuarioEquipoDAL { get; set; }
        public ITareaDAL tareaDAL { get; set; }

        private AgendaVirtualContext _agendaVirtualContext;

        public UnidadDeTrabajo(AgendaVirtualContext agendaVirtualContext, IUsuarioDAL usuarioDAL, IEquipoDAL equipoDAL, IUsuarioEquipoDAL usuarioEquipoDAL, ITareaDAL tareaDAL

            )
        {
            this._agendaVirtualContext = agendaVirtualContext;
            this.usuarioDAL = usuarioDAL;
            this.equipoDAL = equipoDAL;
            this.usuarioEquipoDAL = usuarioEquipoDAL;
            this.tareaDAL = tareaDAL;
        }

        public bool Complete()
        {
            try
            {
                _agendaVirtualContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public void Dispose()
        {
            this._agendaVirtualContext.Dispose();
        }
    }
}
