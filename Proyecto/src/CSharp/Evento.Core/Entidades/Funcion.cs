namespace Evento.Core.Entidades;

public class Funcion
{
    public int idFuncion { get; set; }
    public Eventos evento { get; set; }
    public DateTime fecha { get; set; }

    public Funcion()
    {}
    
    //Métodos
    public bool EsEnFecha(DateTime fechaComparar)
    {
        return fecha.Date == fechaComparar.Date;
    }

    public override string ToString()
    {
        return $"Función {idFuncion} - Evento: {evento.Nombre} - Fecha: {fecha.ToShortDateString()}";
    }
}