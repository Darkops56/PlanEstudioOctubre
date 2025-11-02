using System.Text.Json.Serialization;
using Evento.Core.Services.Enums;

namespace Evento.Core.DTOs
{
    public class TipoEventoDto
    {
        public int idTipoEvento { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ETipoEvento tipoEvento { get; set; }
    }
}