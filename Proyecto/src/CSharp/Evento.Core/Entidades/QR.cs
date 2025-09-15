namespace Evento.Core.Entidades;

public class QR
{
    public string url { get; set; }
    public byte duracion { get; set; }
    public string VCard { get; set; }
    public QR()
    {}

    //Métodos
    public bool EstaVigente(DateTime fechaGeneracion)
    {
        return (DateTime.Now - fechaGeneracion).TotalMinutes <= duracion;
    }

    public override string ToString()
    {
        return $"QR: {url} (Duración: {duracion} min)";
    }
}
