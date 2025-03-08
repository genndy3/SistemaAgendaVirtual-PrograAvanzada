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

        private AgendaVirtualContext _agendaVirtualContext;

        public UnidadDeTrabajo(AgendaVirtualContext agendaVirtualContext

            )
        {
            this._agendaVirtualContext = agendaVirtualContext;

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
