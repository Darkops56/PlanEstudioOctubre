namespace Evento.Core.Entidades;

public class Sector
{
    public int idSector { get; set; }
    public Local local { get; set; }
    public byte Capacidad { get; set; }

    public Sector()
    {}
}