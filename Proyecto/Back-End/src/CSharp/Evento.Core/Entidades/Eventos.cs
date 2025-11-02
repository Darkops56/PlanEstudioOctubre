using Evento.Core.Services.Enums;
using System.Text.Json.Serialization;

namespace Evento.Core.Entidades;
public class Eventos
{
    public int idEvento { get; set; }
    public string Nombre { get; set; }
    public int idTipoEvento { get; set; }
    public TipoEvento tipoEvento { get; set; }
    public DateTime fechaInicio { get; set; }
    public DateTime fechaFin { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EEstados EstadoEvento { get; set; }

    public Eventos()
    { }
}
