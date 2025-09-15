namespace Evento.Core.Entidades;

public class TipoEvento
{
    public int idTipoEvento { get; set; }
    public string tipoEvento { get; set; }
    public TipoEvento()
    {}

    //Métodos
    public override string ToString()
    {
        return $"Tipo de evento: {tipoEvento}";
    }
}
