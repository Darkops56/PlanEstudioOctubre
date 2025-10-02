using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Evento.Core.Services.Utility
{
    public static class UniqueFormatStrings
    {
        public static string NormalizarString(string cadena)
        {
            return cadena.ToLower().Trim();
        }
    }
}