namespace Evento.Core.DTOs
{
    public class RegisterDto
    {
        public string Email { get; set; } = null!;
        public string Contrasena { get; set; } = null!;
        public string Apodo { get; set; } = null!;
        public int DNI { get; set; }
    }
}