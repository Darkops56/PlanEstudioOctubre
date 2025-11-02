using Evento.Core.Services.Enums;
using System.Text.Json.Serialization;

namespace Evento.Core.Entidades;

public class Entrada
{
    public int idEntrada { get; set; }
    public Tarifa tarifa { get; set; }
    public OrdenesCompra ordenesCompra { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EEstados Estado { get; set; }
    public int PrecioPagado { get; set; }
    public Entrada()
    { }
}
