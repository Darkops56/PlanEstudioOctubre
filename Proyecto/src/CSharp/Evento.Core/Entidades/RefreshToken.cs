using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Evento.Core.Entidades
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime Expiracion { get; set; }
        
        public RefreshToken() { }
    }
}