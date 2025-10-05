using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Evento.Core.DTOs
{
    public class StockReservacionesDto
    {
        public int Cantidad { get; set; }
        public int idTarifa { get; set; }
        public int idOrdenCompra { get; set; } 
    }
}