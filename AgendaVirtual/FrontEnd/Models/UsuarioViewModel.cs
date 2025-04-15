namespace FrontEnd.Models
{
    public class UsuarioViewModel
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public string Rol { get; set; } = null!;
        public string Contrasena { get; set; } = null!;
        public DateTime? FechaRegistro { get; set; }
        public string? IdIdentity { get; set; }
    }
}