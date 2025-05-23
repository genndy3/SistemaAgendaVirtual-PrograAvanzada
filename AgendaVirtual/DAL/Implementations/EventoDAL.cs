﻿using DAL.Interfaces;
using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Implementations
{
    public class EventoDAL : DALGenericoImpl<Evento>, IEventoDAL
    {
        private AgendaVirtualContext _context;
        public EventoDAL(AgendaVirtualContext context) : base(context)
        {
            _context = context;
        }
    }

}
