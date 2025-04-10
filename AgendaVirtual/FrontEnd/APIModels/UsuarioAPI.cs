﻿namespace FrontEnd.APIModels
{
    public class UsuarioAPI
    {
        public int IdUsuario { get; set; }

        public string Nombre { get; set; } = null!;

        public string Correo { get; set; } = null!;

        public string Contrasena { get; set; } = null!;

        public string Rol { get; set; } = null!;

        public DateTime? FechaRegistro { get; set; }
    }
}
