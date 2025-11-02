using Evento.Core.Services.Enums;
using System.Text.Json.Serialization;

namespace Evento.Core.Entidades;
public class Tarifa
{
    public int idTarifa { get; set; }
    public Funcion funcion { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ETipoTarifa Tipo { get; set; }
    public int Precio { get; set; }
    public byte Stock { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EEstados Estado { get; set; }

    public Tarifa()
    { }
}
