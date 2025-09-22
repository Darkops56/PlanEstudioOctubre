namespace Evento.Core.Entidades;

public class RegistroCompra
{
    public int idRegistro { get; set; }
    public int idUsuario { get; set; }
    public int idEntrada { get; set; }
    public DateTime Fecha { get; set; }

    public RegistroCompra()
    {}
    
    //Métodos
    public override string ToString()
    {
        return $"Compra #{idRegistro} - Cliente: {idUsuario} - Entrada: {idEntrada} - Fecha: {Fecha}";
    }
}
