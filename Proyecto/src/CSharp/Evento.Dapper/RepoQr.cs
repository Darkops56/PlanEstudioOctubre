using Dapper;
using Evento.Core.Entidades;
using Evento.Core.Services.Repo;
using Evento.Core.Services.Utility;

namespace Evento.Dapper;

public class RepoQR : IRepoQR
{
    private readonly IAdo _ado;
    public RepoQR(IAdo ado) => _ado = ado;

    public async Task<int> InsertQR(QR qr)
    {
        using var db = _ado.GetDbConnection();
        var id = await db.ExecuteScalarAsync<int>(
            @"INSERT INTO QR(idEntrada, url, token, ExpiraEn, VCard, Estado)
              VALUES(@idEntrada, @url, @token, @ExpiraEn, @vcard, @Estado);
              SELECT LAST_INSERT_ID();",
            new
            {
                qr.idEntrada,
                qr.url,
                qr.token,
                qr.ExpiraEn,
                qr.VCard,
                Estado = UniqueFormatStrings.NormalizarString(qr.Estado.ToString()),
            }
        );
        return id;
    }

    public async Task<QR?> ObtenerQRPorEntrada(int idEntrada)
    {
        using var db = _ado.GetDbConnection();
        return await db.QueryFirstOrDefaultAsync<QR>(
            "SELECT idQR, idEntrada, url, token, ExpiraEn, VCard, FechaCreacion, Estado FROM QR WHERE idEntrada = @idEntrada",
            new { idEntrada }
        );
    }

    public async Task<QR?> ObtenerQRPorToken(string token)
    {
        using var db = _ado.GetDbConnection();
        return await db.QueryFirstOrDefaultAsync<QR>(
            "SELECT idQR, idEntrada, url, token, ExpiraEn, VCard, FechaCreacion, Estado FROM QR WHERE token = @token",
            new { token }
        );
    }
}
