namespace Evento.Core.Entidades;

public class QR
{
    public int idQR { get; set; }
    public int idEntrada { get; set; }
    public string url { get; set; } = string.Empty;
    public string? token { get; set; }
    public byte duracion { get; set; } // minutos
    public DateTime FechaCreacion { get; set; }
    public string VCard { get; set; } = string.Empty;

    public QR() { }

    public QR(int idEntrada, string url, byte duracion, string vCard, string? token = null)
    {
        this.idEntrada = idEntrada;
        this.url = url;
        this.duracion = duracion;
        this.VCard = vCard;
        this.token = token;
    }
}
