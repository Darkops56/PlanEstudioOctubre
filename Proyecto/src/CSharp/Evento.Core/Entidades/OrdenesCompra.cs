using Evento.Core.Services.Enums;

namespace Evento.Core.Entidades
{
    public class OrdenesCompra
    {
        public int idOrdenCompra { get; set; }
        public Usuario? usuario { get; set; }
        public DateTime Fecha { get; set; }
        public int Total { get; set; }
        public EMetodoPago metodoPago { get; set; }
        public string? estado { get; set; }
    }
}