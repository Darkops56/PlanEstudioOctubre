namespace Evento.Core.Entidades;

public class Sector
{
    public int idSector { get; set; }
    public Local local { get; set; }
    public byte capacidad { get; set; }
    public List<Eventos> Eventos { get; set; } = new List<Eventos>();

    public Sector(int idSector, Local local, byte capacidad)
    {
        this.idSector = idSector;
        this.local = local;
        this.capacidad = capacidad;
    }

        // Métodos
    public void AgregarEvento(Eventos evento)
    {
        Eventos.Add(evento);
    }

    public bool TieneDisponibilidad()
    {
        return capacidad > 0;
    }
}