namespace Evento.Core.Entidades;
public class Eventos
{
    public int idEvento { get; set; }
    public string Nombre { get; set; }
    public TipoEvento tipoEvento { get; set; }
    public DateTime fechaInicio { get; set; }
    public DateTime fechaFin { get; set; }

    public Eventos(int idEvento, string Nombre, TipoEvento tipoEvento, DateTime fechaInicio, DateTime fechaFin)
    {      
        this.idEvento = idEvento;
        this.Nombre = Nombre;
        this.tipoEvento = tipoEvento;
        this.fechaInicio = fechaInicio;
        this.fechaFin = fechaFin;
    }

    //Métodos
    public bool EstaActivo()
    {
        return DateTime.Now >= fechaInicio && DateTime.Now <= fechaFin;
    }

    public TimeSpan Duracion()
    {
        return fechaFin - fechaInicio;
    }

    public override string ToString()
    {
        return $"Evento: {Nombre} ({tipoEvento.tipoEvento}) - {fechaInicio} a {fechaFin}";
    }
}
