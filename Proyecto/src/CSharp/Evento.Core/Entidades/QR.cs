namespace Evento.Core.Entidades;

public class QR
{
    public string url { get; set; }
    public byte duracion { get; set; }
    public string VCard { get; set; }
    public QR()
    { }

    public QR(string url, byte duracion, string vCard)
    {
        this.url = url;
        this.duracion = duracion;
        this.VCard = vCard;
    }

}
