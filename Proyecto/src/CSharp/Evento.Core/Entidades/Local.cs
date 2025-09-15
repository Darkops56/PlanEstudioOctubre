namespace Evento.Core.Entidades;

public class Local
{
    public int idLocal { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Ubicacion { get; set; }
    public List<Sector> Sectores { get; set; } = new List<Sector>();

    public Local(int idLocal, string Nombre, string Ubicacion)
    {
        this.idLocal = idLocal;
        this.Nombre = Nombre;
        this.Ubicacion = Ubicacion;
    }
    
     // Métodos
    public void AgregarSector(Sector sector)
    {
        Sectores.Add(sector);
    }

    public int CapacidadTotal()
    {
        return Sectores.Sum(s => s.capacidad);
    }

    public void MostrarSectores()
    {
        foreach (var sector in Sectores)
            Console.WriteLine($"Sector: {sector.idSector} - Capacidad: {sector.capacidad}");
    }
} 