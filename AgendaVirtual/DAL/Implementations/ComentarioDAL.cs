using DAL.Interfaces;
using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Implementations
{
    public class ComentarioDAL : DALGenericoImpl<Comentario>, IComentarioDAL
    {
        private AgendaVirtualContext _context;
        public ComentarioDAL(AgendaVirtualContext context) : base(context)
        {
            _context = context;
        }
    }

}
