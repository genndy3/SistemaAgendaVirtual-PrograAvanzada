using DAL.Interfaces;
using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Implementations
{
    public class RecordatorioDAL : DALGenericoImpl<Recordatorio>, IRecordatorioDAL
    {
        private AgendaVirtualContext _context;
        public RecordatorioDAL(AgendaVirtualContext context) : base(context)
        {
            _context = context;
        }
    }
}