using Dapper;
using Evento.Core.Entidades;
using Evento.Core.Services.Repo;

namespace Evento.Dapper
{
    public class RepoLocal : IRepoLocal
    {
        private readonly IAdo _ado;
        public RepoLocal(IAdo ado) => _ado = ado;
        public async Task<bool> DeleteLocal(int id)
        {
            using var db = _ado.GetDbConnection();
            var query = "DELETE FROM Local WHERE idLocal = @idlocal";
            var rows = await db.ExecuteAsync(query, new { idlocal = id });

            return rows > 0;
        }

        public async Task<bool> DeleteSector(int id)
        {
            using var db = _ado.GetDbConnection();
            var query = "DELETE FROM Sector WHERE idSector = @idsector";
            var rows = await db.ExecuteAsync(query, new { idsector = id });

            return rows > 0;
        }

        public async Task<int> InsertLocal(Local local)
        {
            using var db = _ado.GetDbConnection();
            var query = @"
                            INSERT INTO Local(Nombre, Ubicacion)
                            VALUES(@nombre, @ubicacion);
                            SELECT LAST_INSERT_ID();";
            return await db.QuerySingleAsync<int>(query, new
            {
                nombre = local.Nombre,
                ubicacion = local.Ubicacion
            });
        }

        public async Task<int> InsertSector(Sector sector, int id)
        {
            using var db = _ado.GetDbConnection();
            var query = @"
                        INSERT INTO Sector(idLocal, Capacidad)
                        VALUES(@idlocal, @capacidad);
                        SELECT LAST_INSERT_ID();";

            return await db.QuerySingleAsync<int>(query, new
            {
                idlocal = id,
                capacidad = sector.Capacidad
            });
        }

        public async Task<Local?> ObtenerPorId(int id)
        {
            using var db = _ado.GetDbConnection();
            var query = "SELECT * FROM Local WHERE idLocal = @idlocal";

            return await db.QueryFirstOrDefaultAsync<Local?>(query, new { idlocal = id });
        }

        public async Task<IEnumerable<Sector>> ObtenerSectoresDelLocal(int id)
        {
            using var db = _ado.GetDbConnection();
            var query = @"
                            SELECT *
                            FROM Sector s
                            INNER JOIN Local l ON s.idLocal = l.idLocal
                            WHERE s.idLocal = @idlocal";

            var sectores = await db.QueryAsync<Sector, Local, Sector>(
                query,
                (sector, local) =>
                {
                    sector.local = local;
                    return sector;
                },
                new { idlocal = id },
                splitOn: "idLocal"
            );

            return sectores;
        }

        public async Task<Sector?> ObtenerSectorPorId(int id)
        {
            using var db = _ado.GetDbConnection();
            var query = @"
                SELECT *
                FROM Sector s
                INNER JOIN Local l ON s.idLocal = l.idLocal
                WHERE s.idSector = @idsector";

            var result = await db.QueryAsync<Sector, Local, Sector>(
                query,
                (sector, local) =>
                {
                    sector.local = local;
                    return sector;
                },
                new { idsector = id },
                splitOn: "idLocal"
            );

            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<Local>> ObtenerTodos()
        {
            using var db = _ado.GetDbConnection();
            var query = "SELECT * FROM Local";
            return await db.QueryAsync<Local>(query);
        }

        public async Task<bool> UpdateLocal(Local local)
        {
            using var db = _ado.GetDbConnection();
            var query = "UPDATE Local SET Nombre = @nombre, Ubicacion = @ubicacion WHERE idLocal = @idlocal";
            var rows = await db.ExecuteAsync(query, new
            {
                nombre = local.Nombre,
                ubicacion = local.Ubicacion,
                idlocal = local.idLocal
            });

            return rows > 0;
        }
        public async Task<bool> UpdateSector(Sector sector, int id)
        {
            using var db = _ado.GetDbConnection();
            var query = "UPDATE Sector SET idLocal = @idlocal, Capacidad = @capacidad WHERE idSector = @idsector";
            var rows = await db.ExecuteAsync(query, new
            {
                idsector = sector.idSector,
                capacidad = sector.Capacidad,
                idlocal = id
            });

            return rows > 0;
        }
    }
}