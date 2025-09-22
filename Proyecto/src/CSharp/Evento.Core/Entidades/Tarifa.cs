namespace Evento.Core.Entidades;
public class Tarifa
{
    public int idTarifa { get; set; }
    public string Tipo { get; set; }
    public int Precio { get; set; }
    public byte Stock { get; set; }
    public bool Estado { get; set; }

    public Tarifa()
    { }

    //Métodos
    public void AplicarDescuento(int porcentaje)
    {
        if (porcentaje > 0 && porcentaje <= 100)
            Precio -= (Precio * porcentaje) / 100;
    }
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
        return $"Tarifa {Tipo}";
    }
}
