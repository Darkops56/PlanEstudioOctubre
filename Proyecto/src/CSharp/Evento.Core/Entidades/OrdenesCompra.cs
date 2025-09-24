using Evento.Core.Services;

namespace Evento.Core.Entidades
{
    public class OrdenesCompra
    {
        public int idOrdenCompra { get; set; }
        public Usuario? usuario { get; set; }
        public DateTime Fecha { get; set; }
        public byte Total { get; set; }
        public EMetodoPago metodoPago { get; set; }
        public string? estado { get; set; }
    }
}