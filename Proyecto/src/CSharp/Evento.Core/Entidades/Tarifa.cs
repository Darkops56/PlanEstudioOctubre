namespace Evento.Core.Entidades;
public class Tarifa
{
    public int idTarifa { get; set; }
    public byte Stock { get; set; }
    public string Tipo { get; set; }

    public Tarifa()
    {}

    //Métodos
        public bool HayStock()
    {
        return Stock > 0;
    }

    public void ReducirStock()
    {
        if (Stock > 0) Stock--;
    }

    public override string ToString()
    {
        return $"Tarifa {Tipo} - Stock: {Stock}";
    }
}
