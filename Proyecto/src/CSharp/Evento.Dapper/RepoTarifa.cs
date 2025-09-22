using Dapper;
using Evento.Core.Entidades;
using Evento.Core.Services;
using Mysqlx.Resultset;

namespace Evento.Dapper
{
    public class RepoTarifa : IRepoTarifa
    {
        private readonly Ado _ado;
        public async Task<int> InsertTarifa(Tarifa tarifa)
        {
            var db = _ado.GetDbConnection();
            var query = "INSERT INTO(Tipo, Precio, Stock, Estado) VALUES(@tipo, @precio, @stock, FALSE)";

            return await db.ExecuteAsync(query, new
            {
                tipo = tarifa.Tipo,
                precio = tarifa.Precio,
                stock = tarifa.Stock
            });
        }
        public async Task<Tarifa?> ObtenerPorId(int id)
        {
            var db = _ado.GetDbConnection();
            var query = "SELECT * FROM Tarifa WHERE idTarifa = @idtarifa";

            return await db.QueryFirstAsync<Tarifa?>(query, new{ idtarifa = id });
        }
        public async Task<IEnumerable<Tarifa>> ObtenerTodos()
        {
            var db = _ado.GetDbConnection();
            var query = "SELECT * FROM Tarifa";

            return await db.QueryAsync<Tarifa>(query);
        }

        public async Task<bool> UpdateTarifa(Tarifa tarifa)
        {
            using var db = _ado.GetDbConnection();
            var query = "UPDATE Tarifa SET Tipo = @tipo, Precio = @precio, Stock = @stock, Estado = @estado WHERE idTarifa = @idtarifa ";
            var rows = await db.ExecuteAsync(query, new
            {
                idtarifa = tarifa.idTarifa,
                tipo = tarifa.Tipo,
                precio = tarifa.Precio,
                stock = tarifa.Stock,
                estado = tarifa.Estado
            });
            return rows > 0;
        }
    }
}