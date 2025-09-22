using Dapper;
using Evento.Core.Entidades;
using Evento.Core.Services;

namespace Evento.Dapper
{
    public class RepoFuncion : IRepoFuncion
    {
        private readonly Ado _ado;
        public async Task<bool> DeleteFuncion(int id)
        {
            var db = _ado.GetDbConnection();
            var query = "DELETE FROM Funcion WHERE idFuncion = @idfuncion";
            var rows = await db.ExecuteAsync(query, new { idfuncion = id });
            
            return rows > 0;
        }

        public async Task<int> InsertFuncion(Funcion funcion)
        {
            var db = _ado.GetDbConnection();
            string query = "INSERT INTO Funcion(idEvento, Fecha) VALUES(@idevento, @fecha)";
            var rows = await db.ExecuteAsync(query, new { idevento = funcion.evento.idEvento, fecha = funcion.fecha });
            return rows > 0 ? rows : 0; 
        }

        public async Task<IEnumerable<Tarifa>> ObtenerTarifasDeFuncion(int id)
        {
            var db = _ado.GetDbConnection();
            var query = "SELECT * FROM Tarifa JOIN Entrada USING (idTarifa) WHERE Entrada.idFuncion = @idfuncion";

            return await db.QueryAsync<Tarifa>(query, new{ idfuncion = id });
        }

        public async Task<Funcion?> ObtenerPorId(int id)
        {
            var db = _ado.GetDbConnection();
            var query = "SELECT * FROM Funcion WHERE idFuncion = @idfuncion";
            
            return await db.QueryFirstAsync<Funcion>(query, new { idfuncion = id });
        }

        public async Task<IEnumerable<Funcion>> ObtenerTodos()
        {
            var db = _ado.GetDbConnection();
            var query = "SELECT * FROM Funcion";

            return await db.QueryAsync<Funcion>(query);
        }

        public async Task<bool> UpdateFuncion(Funcion funcion)
        {
            var db = _ado.GetDbConnection();
            var query = "UPDATE Funcion SET idEvento = @idevento, Fecha = @fecha";

            var rows = await db.ExecuteAsync(query, new
            {
                idevento = funcion.evento.idEvento,
                fecha = funcion.fecha
            });

            return rows > 0;
        }
    }
}