namespace Evento.Core.Entidades;

public class Entrada
{
    public int idEntrada { get; set; }
    public Tarifa tarifa { get; set; }
    public OrdenesCompra ordenesCompra { get; set; }
    public Entrada()
    { }
    //Métodos
    
    public string MostrarDetalle()
    {
        return $"Entrada {idEntrada} - Tarifa: {tarifa.Tipo}";
    }
}
