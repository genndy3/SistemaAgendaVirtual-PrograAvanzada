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
        public IUsuarioDAL UsuarioDAL { get; set; }

        private AgendaVirtualContext _agendaVirtualContext;

        public UnidadDeTrabajo(AgendaVirtualContext agendaVirtualContext, IUsuarioDAL usuarioDAL

            )
        {
            this._agendaVirtualContext = agendaVirtualContext;
            this.UsuarioDAL = usuarioDAL;
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
