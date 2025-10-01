using Evento.Core.Services.Enums;

namespace Evento.Core.Entidades;

public class Funcion
{
    public int idFuncion { get; set; }
    public Eventos evento { get; set; }
    public DateTime Fecha { get; set; }
    public EEstados Estado { get; set; }

    public Funcion()
    { }
}