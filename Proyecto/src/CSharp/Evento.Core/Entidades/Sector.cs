namespace Evento.Core.Entidades;

public class Sector
{
    public int idSector { get; set; }
    public Local local { get; set; }
    public byte Capacidad { get; set; }
    public List<Eventos> Eventos { get; set; } = new List<Eventos>();

    public Sector()
    {}

    // Métodos
    public void AgregarEvento(Eventos evento)
    {
        Eventos.Add(evento);
    }

    public bool TieneDisponibilidad()
    {
        return Capacidad > 0;
    }
}