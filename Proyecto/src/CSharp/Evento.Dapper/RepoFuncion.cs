using Dapper;
using Evento.Core.Entidades;
using Evento.Core.Services.Repo;

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
            var query = "SELECT * FROM Tarifa WHERE idFuncion = @idfuncion";

            return await db.QueryAsync<Tarifa>(query, new{ idfuncion = id });
        }

        public async Task<Funcion?> ObtenerPorId(int id)
        {
            var db = _ado.GetDbConnection();
            var query = "SELECT * FROM Funcion WHERE idFuncion = @idfuncion";
            
            return await db.QueryFirstOrDefaultAsync<Funcion>(query, new { idfuncion = id });
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
            var query = "UPDATE Funcion SET idEvento = @idevento, Fecha = @fecha where idFuncion = @idfuncion";

            var rows = await db.ExecuteAsync(query, new
            {
                idevento = funcion.evento.idEvento,
                fecha = funcion.fecha,
                idfuncion = funcion.idFuncion
            });

            return rows > 0;
        }

        public async Task<string> CancelarFuncion(int id)
        {
            var db = _ado.GetDbConnection();

            var funcion = await ObtenerPorId(id);
            if (funcion == null)
                throw new Exception("La función no existe");

            if (funcion.Estado == "Cancelada")
                throw new Exception("La función ya está cancelada");

            var rows = await db.ExecuteAsync(
                "UPDATE Funcion SET Estado = 'Cancelada' WHERE idFuncion = @IdFuncion",
                new { IdFuncion = id });

            return rows > 0 ? "Función cancelada correctamente" : "No se pudo cancelar la función";
        }
    }
}