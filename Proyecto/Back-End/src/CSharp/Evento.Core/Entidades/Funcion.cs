using Evento.Core.Services.Enums;
using System.Text.Json.Serialization;

namespace Evento.Core.Entidades;

public class Funcion
{
    public int idFuncion { get; set; }
    public string Nombre { get; set; }
    public Eventos evento { get; set; }
    public DateTime Fecha { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EEstados Estado { get; set; }
    public Funcion()
    { }
}