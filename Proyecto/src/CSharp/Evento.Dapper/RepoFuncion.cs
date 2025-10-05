using Dapper;
using Evento.Core.Entidades;
using Evento.Core.Services.Enums;
using Evento.Core.Services.Repo;
using Evento.Core.Services.Utility;

namespace Evento.Dapper
{
    public class RepoFuncion : IRepoFuncion
    {
        private readonly IAdo _ado;
        public RepoFuncion(IAdo ado) => _ado = ado;
        public async Task<bool> DeleteFuncion(int id)
        {
            using var db = _ado.GetDbConnection();
            var query = "DELETE FROM Funcion WHERE idFuncion = @idfuncion";
            var rows = await db.ExecuteAsync(query, new { idfuncion = id });

            return rows > 0;
        }

        public async Task<int> InsertFuncion(Funcion funcion)
        {
            using var db = _ado.GetDbConnection();
            string query = @"INSERT INTO Funcion(idEvento, Fecha, Estado, Nombre)
                            VALUES(@idevento, @fecha, @estado, @nombre);
                            SELECT LAST_INSERT_ID();";

            var id = await db.QuerySingleAsync<int>(query, new
            {
                idevento = funcion.evento!.idEvento,
                fecha = funcion.Fecha,
                estado = funcion.Estado.ToString(),
                nombre = funcion.Nombre
            });

            return id;
        }

        public async Task<IEnumerable<Tarifa>> ObtenerTarifasDeFuncion(int id)
        {
            using var db = _ado.GetDbConnection();
            var query = "SELECT * FROM Tarifa WHERE idFuncion = @idfuncion";

            return await db.QueryAsync<Tarifa>(query, new { idfuncion = id });
        }

        public async Task<Funcion?> ObtenerPorId(int id)
        {
            using var db = _ado.GetDbConnection();
            var query = @"
                            SELECT f.idFuncion, f.idEvento, f.Fecha, f.Estado, f.Nombre,
                                e.idEvento, e.Nombre, e.idTipoEvento, e.fechaInicio, e.fechaFin, e.Estado AS EstadoEvento
                            FROM Funcion f
                            INNER JOIN Evento e ON f.idEvento = e.idEvento
                            WHERE f.idFuncion = @idfuncion";

            var result = await db.QueryAsync<Funcion, Eventos, Funcion>(
                query,
                (funcion, evento) =>
                {
                    funcion.evento = evento;
                    return funcion;
                },
                new { idfuncion = id },
                splitOn: "idEvento"
            );

            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<Funcion>> ObtenerTodos()
        {
            using var db = _ado.GetDbConnection();
            var query = @"
                            SELECT f.Nombre, f.idFuncion, f.idEvento, f.Fecha, f.Estado,
                                e.idEvento, e.Nombre, e.idTipoEvento, e.fechaInicio, e.fechaFin, e.Estado AS EstadoEvento
                            FROM Funcion f
                            INNER JOIN Evento e ON f.idEvento = e.idEvento";

            var result = await db.QueryAsync<Funcion, Eventos, Funcion>(
                query,
                (funcion, evento) =>
                {
                    funcion.evento = evento;
                    return funcion;
                },
                splitOn: "idEvento"
            );

            return result;
        }

        public async Task<bool> UpdateFuncion(Funcion funcion)
        {
            using var db = _ado.GetDbConnection();
            var query = "UPDATE Funcion SET idEvento = @idevento, Fecha = @fecha, Nombre = @nombre where idFuncion = @idfuncion";

            var rows = await db.ExecuteAsync(query, new
            {
                idevento = funcion.evento.idEvento,
                fecha = funcion.Fecha,
                idfuncion = funcion.idFuncion,
                nombre = funcion.Nombre,
            });

            return rows > 0;
        }

        public async Task<string> CancelarFuncion(int idFuncion)
        {
            using var db = _ado.GetDbConnection();
            var funcion = await ObtenerPorId(idFuncion);
            if (funcion == null)
                return "Función no encontrada";
                
            db.Open();
            using var tran = db.BeginTransaction();

            try
            {
                var entradas = await db.QueryAsync<Entrada>(
                    "SELECT * FROM Entrada WHERE idFuncion = @idFuncion",
                    new { idFuncion },
                    tran
                );

                foreach (var entrada in entradas)
                {
                    await db.ExecuteAsync(
                        "UPDATE Tarifa SET Stock = Stock + 1 WHERE idTarifa = @idTarifa",
                        new { idTarifa = entrada.tarifa.idTarifa },
                        tran
                    );

                    await db.ExecuteAsync(
                        "UPDATE Entrada SET Estado = 'Anulada' WHERE idEntrada = @idEntrada",
                        new { idEntrada = entrada.idEntrada },
                        tran
                    );
                }

                await db.ExecuteAsync(
                    "UPDATE Funcion SET Estado = 'Cancelada' WHERE idFuncion = @idFuncion",
                    new { idFuncion },
                    tran
                );

                tran.Commit();
                return string.Empty;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                return ex.Message;
            }
        }


        public async Task<EEstados> ObtenerEstadoFuncion(string estadoFuncion)
        {
            using var db = _ado.GetDbConnection();
            string query = "SELECT Estado FROM Funcion WHERE Estado = @estado";

            var estadoStr = await db.QueryFirstOrDefaultAsync<string>(query, new
            {
                estado = UniqueFormatStrings.NormalizarString(estadoFuncion)
            });

            if (string.IsNullOrEmpty(estadoStr))
                throw new Exception("Función no encontrada");

            return Enum.Parse<EEstados>(estadoStr);
        }
    }
}