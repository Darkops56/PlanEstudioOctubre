using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Evento.Core.DTOs
{
    public class RegisterDto
    {
        public string Email { get; set; } = null!;
        public string Contrasena { get; set; } = null!;
        public string Apodo { get; set; } = null!;
        public ClienteDto cliente { get; set; } = null!;
    }
}