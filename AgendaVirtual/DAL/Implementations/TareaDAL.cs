using DAL.Interfaces;
using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Implementations
{
    public class TareaDAL : DALGenericoImpl<Tarea>, ITareaDAL
    {
        private AgendaVirtualContext _context;
        public TareaDAL(AgendaVirtualContext context) : base(context)
        {
            _context = context;
        }
    }
}
