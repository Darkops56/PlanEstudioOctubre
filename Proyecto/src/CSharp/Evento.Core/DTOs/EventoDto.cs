namespace Evento.Core.DTOs;

public class EventoDto
{
    public string Nombre { get; set; }
    public string tipoEvento { get; set; }
    public DateTime fechaInicio { get; set; }
    public DateTime fechaFin { get; set; }
    public EventoDto() {}
}
