using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interfaces;
using Entities.Entities;

namespace DAL.Implementations
{
    public class EquipoDAL : DALGenericoImpl<Equipo>, IEquipoDAL
    {
        private AgendaVirtualContext _context;
        public EquipoDAL(AgendaVirtualContext context) : base(context)
        {
            _context = context;
        }
    }
}
