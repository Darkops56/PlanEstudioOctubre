namespace Evento.Core.Entidades;

public class Local
{
    public int idLocal { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Ubicacion { get; set; }
    public List<Sector>? Sectores { get; set; } = new List<Sector>();

    public Local()
    {}
    
     // Métodos
    public void AgregarSector(Sector sector)
    {
        Sectores.Add(sector);
    }

    public int CapacidadTotal()
    {
        return Sectores.Sum(s => s.Capacidad);
    }

    public void MostrarSectores()
    {
        foreach (var sector in Sectores)
            Console.WriteLine($"Sector: {sector.idSector} - Capacidad: {sector.Capacidad}");
    }
} 