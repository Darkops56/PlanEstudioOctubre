namespace Evento.Core.Entidades;

public class Entrada
{
    public int idEntrada { get; set; }
    public int Precio { get; set; }
    public Eventos evento { get; set; }
    public Tarifa tarifa { get; set; }

    public Entrada()
    {}
    //Métodos
        public void AplicarDescuento(int porcentaje)
    {
        if (porcentaje > 0 && porcentaje <= 100)
            Precio -= (Precio * porcentaje) / 100;
    }

    public string MostrarDetalle()
    {
        return $"Entrada {idEntrada} - Evento: {evento.Nombre} - Precio: ${Precio} - Tarifa: {tarifa.Tipo}";
    }
}
