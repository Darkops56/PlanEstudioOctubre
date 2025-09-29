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

    //Métodos
    public bool EstaActivo()
    {
        return DateTime.Now >= fechaInicio && DateTime.Now <= fechaFin;
    }

    public TimeSpan Duracion()
    {
        return fechaFin - fechaInicio;
    }
    public string Publicar()
    {
        if (EstadoEvento == EEstados.Publicado)
        {
            return "El evento ya fue publicado";
        }
        else if (EstadoEvento == EEstados.Cancelado)
        {
            return "El evento fue cancelado con anterioridad";
        }
        EstadoEvento = EEstados.Publicado;
        return "El evento fue Publicado correctamente.";
    }
    public string Cancelar()
    {
        if (EstadoEvento == EEstados.Cancelado)
        {
            return "El evento ya ha sido cancelado";
        }
        EstadoEvento = EEstados.Cancelado;
        return "El evento fue cancelado con éxito.";
    }

    public override string ToString()
    {
        return $"Evento: {Nombre} ({tipoEvento.tipoEvento}) - {fechaInicio} a {fechaFin}";
    }
}
