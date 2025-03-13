using DAL.Interfaces;
using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Implementations
{
    public class UsuarioEquipoDAL : DALGenericoImpl<UsuarioEquipo>, IUsuarioEquipoDAL
    {
        private AgendaVirtualContext _context;

        public UsuarioEquipoDAL(AgendaVirtualContext context): base(context)
        {
            _context = context;
        }
    }
}
