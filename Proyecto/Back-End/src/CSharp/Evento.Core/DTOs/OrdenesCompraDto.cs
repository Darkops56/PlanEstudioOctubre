namespace Evento.Core.DTOs;

public class OrdenesCompraDto
{
    public string Email { get; set; }
    public DateTime Fecha { get; set; }
    public int Total { get; set; }
    public string metodoPago { get; set; }
    public string Estado { get; set; }
    
}
