namespace Evento.Core.Entidades;

public class QR
{
    public string url { get; set; }
    public byte duracion { get; set; }
    public string VCard { get; set; }
<<<<<<< HEAD
    public QR()
    {}
=======

    public QR(string url, byte duracion, string vCard)
    {
        this.url = url;
        this.duracion = duracion;
        this.VCard = vCard;
    }
>>>>>>> 4a21e4f6bb4ad7ee1f110ae8863f0d0de88c95d0

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
