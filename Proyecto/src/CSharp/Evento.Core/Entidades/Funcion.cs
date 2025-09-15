namespace Evento.Core.Entidades;

public class Funcion
{
    public int idFuncion { get; set; }
    public Eventos evento { get; set; }
    public DateTime fecha { get; set; }

    public Funcion(int idFuncion, Eventos evento, DateTime fecha)
    {
        this.idFuncion = idFuncion;
        this.evento = evento;
        this.fecha = fecha;
    }
    
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