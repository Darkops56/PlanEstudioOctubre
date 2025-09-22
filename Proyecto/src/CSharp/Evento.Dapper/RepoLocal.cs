using Dapper;
using Evento.Core.Entidades;
using Evento.Core.Services;

namespace Evento.Dapper
{
    public class RepoLocal : IRepoLocal
    {
        private readonly Ado _ado;
        public async Task<bool> DeleteLocal(int id)
        {
            var db = _ado.GetDbConnection();
            var query = "DELETE FROM Local WHERE idLocal = @idlocal";
            var rows = await db.ExecuteAsync(query, new { idlocal = id });

            return rows > 0;
        }

        public async Task<bool> DeleteSector(int id)
        {
            var db = _ado.GetDbConnection();
            var query = "DELETE FROM Sector WHERE idSector = @idsector";
            var rows = await db.ExecuteAsync(query, new { idsector = id });

            return rows > 0;
        }

        public async Task<int> InsertLocal(Local local)
        {
            var db = _ado.GetDbConnection();
            var query = "INSERT INTO(Nombre, Ubicacion) VALUES(@nombre, @ubicacion)";
            return await db.ExecuteAsync(query, new{ nombre = local.Nombre, ubicacion = local.Ubicacion });
        }

        public async Task<int> InsertSector(Sector sector, int id)
        {
            var db = _ado.GetDbConnection();
            var query = "INSERT INTO Sector(idLocal, Capacidad) VALUES(@idlocal, @capacidad)";

            return await db.ExecuteAsync(query, new
            {
                idlocal = id,
                capacidad = sector.capacidad
            });

        }

        public async Task<Local?> ObtenerPorId(int id)
        {
            var db = _ado.GetDbConnection();
            var query = "SELECT * FROM Local WHERE idLocal = @idlocal";

            return await db.QueryFirstAsync<Local?>(query, new { idlocal = id });
        }

        public async Task<IEnumerable<Sector>> ObtenerSectoresDelLocal(int id)
        {
            var db = _ado.GetDbConnection();
            var query = "SELECT * FROM Sector JOIN Local USING (idLocal) WHERE Sector.idLocal = @idlocal";

            return await db.QueryAsync<Sector>(query, new { idlocal = id });
        }

        public async Task<IEnumerable<Local>> ObtenerTodos()
        {
            var db = _ado.GetDbConnection();
            var query = "SELECT * FROM Local";
            return await db.QueryAsync<Local>(query);
        }

        public async Task<bool> UpdateLocal(Local local)
        {
            var db = _ado.GetDbConnection();
            var query = "UPDATE Local SET Nombre = @nombre, Ubicacion = @ubicacion WHERE idLocal = @idlocal";
            var rows = await db.ExecuteAsync(query, new
            {
                nombre = local.Nombre,
                ubicacion = local.Ubicacion,
                idlocal = local.idLocal
            });

            return rows > 0;
        }

        public async Task<bool> UpdateSector(Sector sector)
        {
            var db = _ado.GetDbConnection();
            var query = "UPDATE Sector SET idLocal = @idlocal, Capacidad = @capacidad WHERE idSector = @idsector";
            var rows = await db.ExecuteAsync(query, new
            {
                idsector = sector.idSector,
                capacidad = sector.capacidad,
                idlocal = sector.local.idLocal
            });

            return rows > 0;
        }
    }
}