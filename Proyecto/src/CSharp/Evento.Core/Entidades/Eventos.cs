using Evento.Core.Services.Enums;

namespace Evento.Core.Entidades;
public class Eventos
{
    public int idEvento { get; set; }
    public string Nombre { get; set; }
    public TipoEvento tipoEvento { get; set; }
    public DateTime fechaInicio { get; set; }
    public DateTime fechaFin { get; set; }
    public EEstados EstadoEvento { get; set; }

    public Eventos()
    { }
}
