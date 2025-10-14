using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Evento.Core.Services.Enums;

namespace Evento.Core.DTOs
{
    public class TipoEventoDto
    {
        public int idTipoEvento { get; set; }
        public ETipoEvento tipoEvento { get; set; }
    }
}