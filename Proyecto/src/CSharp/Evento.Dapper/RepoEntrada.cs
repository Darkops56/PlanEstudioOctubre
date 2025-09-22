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
            return rows > 0;
        }

        public async Task<int> InsertEntrada(Entrada entrada)
        {
            var db = _ado.GetDbConnection();
            return await db.ExecuteAsync("INSERT INTO Entrada(idEntrada, idEvento, idTarifa) VALUES(@identrada, @idfuncion, @tarifa)", new
            {
                identrada = entrada.idEntrada,
                idfuncion = entrada.funcion.idFuncion,
                idtarifa = entrada.tarifa.idTarifa,
            });
        }

        public async Task<Entrada?> ObtenerEntrada(int id)
        {
            var db = _ado.GetDbConnection();
            return await db.QueryFirstOrDefaultAsync<Entrada?>("SELECT * FROM Entrada WHERE idEntrada = @Id", new{ Id = id });
        }
        public async Task<IEnumerable<Entrada>> ObtenerTodos()
        {
            var db = _ado.GetDbConnection();
            return await db.QueryAsync<Entrada>("SELECT * FROM Entrada");
        }
    }
}