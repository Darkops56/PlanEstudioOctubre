using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Evento.Core.DTOs
{
    public class ClienteDto
    {
        public string nombreCompleto { get; set; } = null!;
        public int DNI { get; set; }
        public string? Telefono { get; set; }
    }
}