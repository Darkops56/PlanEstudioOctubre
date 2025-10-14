using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Evento.Core.Entidades;

namespace Evento.Core.DTOs
{
    public class FuncionTarifaDto
    {
        public Funcion funcion { get; set; }
        public Tarifa tarifa { get; set; }
    }
}