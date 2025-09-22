namespace Evento.Core.Entidades;

public class Entrada
{
    public int idEntrada { get; set; }
    public Funcion funcion { get; set; }
    public Tarifa tarifa { get; set; }

    public Entrada()
    { }
    //Métodos
    
    public string MostrarDetalle()
    {
        return $"Entrada {idEntrada} - Evento: {funcion.evento.Nombre} - Tarifa: {tarifa.Tipo}";
    }
}
