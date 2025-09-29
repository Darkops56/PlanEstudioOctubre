using Evento.Core.Services.Enums;

namespace Evento.Core.Entidades;

public class Funcion
{
    public int idFuncion { get; set; }
    public Eventos evento { get; set; }
    public DateTime Fecha { get; set; }
    public EEstados Estado { get; set; }

    public Funcion()
    { }
    
    //Métodos
    public bool EsEnFecha(DateTime fechaComparar)
    {
        return Fecha.Date == fechaComparar.Date;
    }
    public override string ToString()
    {
        return $"Función {idFuncion} - Evento: {evento.Nombre} - Fecha: {Fecha.ToShortDateString()}";
    }
}