using Evento.Core.Services.Enums;
using System.Text.Json.Serialization;

namespace Evento.Core.Entidades;

public class QR
{
    public int idQR { get; set; }
    public int idEntrada { get; set; }
    public string url { get; set; } = string.Empty;
    public string? token { get; set; }
    public DateTime ExpiraEn { get; set; }
    public DateTime FechaCreacion { get; set; }
    public string VCard { get; set; } = string.Empty;
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EEstados Estado { get; set; }

    public QR() { }

    public QR(int idEntrada, string url, DateTime ExpiraEn, string vCard, string? token = null, EEstados estado = EEstados.Creado)
    {
        this.idEntrada = idEntrada;
        this.url = url;
        this.ExpiraEn = ExpiraEn;
        this.VCard = vCard;
        this.token = token;
        this.Estado = estado;
    }
}
