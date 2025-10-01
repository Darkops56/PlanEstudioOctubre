using Evento.Core.Services.Enums;

namespace Evento.Core.Entidades;
public class Tarifa
{
    public int idTarifa { get; set; }
    public Funcion funcion { get; set; }
    public ETipoTarifa Tipo { get; set; }
    public int Precio { get; set; }
    public byte Stock { get; set; }
    public bool Estado { get; set; }

    public Tarifa()
    { }
}
