using MySql.Data.MySqlClient;
using Dapper;
using Evento.Core.Entidades;
using Evento.Core.Services;

namespace Evento.Dapper
{
    public class RepoEntrada : IRepoEntrada
    {
        private readonly IAdo _ado;
        public RepoEntrada(IAdo ado) => _ado = ado;
        public async Task<bool> DeleteEntrada(int id)
        {
            var db = _ado.GetDbConnection();
            var rows = await db.ExecuteAsync("DELETE FROM Entrada WHERE idEntrada = @Id", new { Id = id });
            if (rows == 0)
            {
                return false;
            }
            return true;
        }

        public async Task<int> InsertEntrada(Entrada entrada)
        {
            var db = _ado.GetDbConnection();
            return await db.ExecuteAsync("INSERT INTO Entrada(idEntrada, Precio, idEvento, idTarifa) VALUES(@identrada, @precio, @evento, @tarifa)", new
            {
                identrada = entrada.idEntrada,
                precio = entrada.Precio,
                idevento = entrada.evento.idEvento,
                idtarifa = entrada.tarifa.idTarifa
            });
        }

        public async Task<Entrada?> ObtenerEntrada(int id)
        {
            var db = _ado.GetDbConnection();
            return await db.QueryFirstOrDefaultAsync<Entrada?>("SELECT * FROM Entrada WHERE idEntrada = @Id", new{ Id = id });
        }

        public async Task<Entrada?> ObtenerEntradaConQR(int idEntrada)
        {
            var db = _ado.GetDbConnection();
            string query = "SELECT * FROM QR WHERE idEntrada = @Id";
            return await db.QueryFirstOrDefaultAsync<Entrada?>(query, new{ Id = idEntrada });
        }

        public async Task<IEnumerable<Entrada>> ObtenerTodos()
        {
            var db = _ado.GetDbConnection();
            return await db.QueryAsync<Entrada>("SELECT * FROM Entrada");
        }

        public async Task<bool> UpdateEntrada(Entrada entrada)
        {
            var db = _ado.GetDbConnection();
            string query = "UPDATE Entrada SET idEntrada = @identrada, Precio = @precio, idEvento = @idevento, idTarifa = @idtarifa";
            var rows = await db.ExecuteAsync(query, new
            {
                identrada = entrada.idEntrada,
                precio = entrada.Precio,
                idevento = entrada.evento.idEvento,
                idtarifa = entrada.tarifa.idTarifa
            });
            if (rows == 0)
            {
                return false;
            }
            return true;
        }

        public Task<bool> ValidarQR(string qrCodigo)
        {
            throw new NotImplementedException();
        }
    }
}