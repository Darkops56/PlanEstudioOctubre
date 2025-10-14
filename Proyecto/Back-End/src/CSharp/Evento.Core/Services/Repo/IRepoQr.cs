using System.Data;
using Evento.Core.Entidades;

namespace Evento.Core.Services.Repo;
public interface IRepoQR
{
    Task<int> InsertQR(QR qr, IDbConnection db, IDbTransaction transaction);
    Task<QR?> ObtenerQRPorEntrada(int idEntrada);
    Task<QR?> ObtenerQRPorToken(string token);
}
