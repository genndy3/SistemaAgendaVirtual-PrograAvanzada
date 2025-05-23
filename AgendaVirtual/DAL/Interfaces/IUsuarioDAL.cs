﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Entities;

namespace DAL.Interfaces
{
    public interface IUsuarioDAL : IDALGenerico<Usuario>
    {
        public bool UpdateSP(Usuario usuario);
        public bool DeleteSP(Usuario usuario);
        public int GetIdUser(string id);
    }
}
