namespace Evento.Core.Entidades;

public class StockReservaciones
{
    public int idStockReservacion { get; set; }
    public int Cantidad { get; set; }
    public DateTime fechReserva { get; set; }
    public DateTime expiraEn { get; set; }
    public int idTarifa { get; set; }
    public int idOrdenCompra { get; set; } 
    
    public StockReservaciones() {}
}
