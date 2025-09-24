namespace Evento.Core.Entidades;
public class Eventos
{
    public int idEvento { get; set; }
    public string Nombre { get; set; }
    public TipoEvento tipoEvento { get; set; }
    public DateTime fechaInicio { get; set; }
    public DateTime fechaFin { get; set; }
    public string EstadoEvento { get; set; }

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
        if (EstadoEvento == "Publicado")
        {
            return "El evento ya fue publicado";
        }
        else if (EstadoEvento == "Cancelado")
        {
            return "El evento fue cancelado con anterioridad";
        }
        return EstadoEvento = "Publicado";
    }
    public string Cancelar()
    {
        if (EstadoEvento == "Cancelado")
        {
            return "El evento ya ha sido cancelado";
        }
        return EstadoEvento = "Cancelado";
    }

    public override string ToString()
    {
        return $"Evento: {Nombre} ({tipoEvento.tipoEvento}) - {fechaInicio} a {fechaFin}";
    }
}
