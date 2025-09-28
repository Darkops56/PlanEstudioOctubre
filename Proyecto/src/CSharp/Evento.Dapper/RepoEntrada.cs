using MySql.Data.MySqlClient;
using Dapper;
using Evento.Core.Entidades;
using Evento.Core.Services.Repo;

namespace Evento.Dapper
{
    public class RepoEntrada : IRepoEntrada
    {
        private readonly IAdo _ado;
        public RepoEntrada(IAdo ado) => _ado = ado;

        public Task<string> AnularEntrada(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteEntrada(int id)
        {
            var db = _ado.GetDbConnection();
            var rows = await db.ExecuteAsync("DELETE FROM Entrada WHERE idEntrada = @Id", new { Id = id });
            return rows > 0;
        }

        public async Task<int> InsertEntrada(Entrada entrada)
        {
            if (entrada.tarifa == null) throw new Exception("Tarifa no puede ser null");
            if (entrada.ordenesCompra == null) throw new Exception("OrdenCompra no puede ser null");


            var db = _ado.GetDbConnection();
            var idGenerado = await db.ExecuteScalarAsync<int>("INSERT INTO Entrada(idTarifa, idOrdenCompra) VALUES(@idtarifa, @idordencompra); SELECT LAST_INSERT_ID();",
            new
            {
                idtarifa = entrada.tarifa.idTarifa,
                idordencompra = entrada.ordenesCompra.idOrdenCompra
            }
            );
            entrada.idEntrada = idGenerado;
            return idGenerado;
        }

        public async Task<Entrada?> ObtenerEntrada(int id)
        {
            var db = _ado.GetDbConnection();
            return await db.QueryFirstOrDefaultAsync<Entrada?>("SELECT * FROM Entrada WHERE idEntrada = @Id", new { Id = id });
        }
        public async Task<IEnumerable<Entrada>> ObtenerTodos()
        {
            var db = _ado.GetDbConnection();
            return await db.QueryAsync<Entrada>("SELECT * FROM Entrada");
        }
    }
}