using Dapper;
using Evento.Core.Entidades;
using Evento.Core.Services.Enums;
using Evento.Core.Services.Repo;

namespace Evento.Dapper
{
    public class RepoFuncion : IRepoFuncion
    {
        private readonly IAdo _ado;
        public RepoFuncion(IAdo ado) => _ado = ado;
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
            string query = @"INSERT INTO Funcion(idEvento, Fecha, Estado)
                            VALUES(@idevento, @fecha, @estado);
                            SELECT LAST_INSERT_ID();";

            var id = await db.ExecuteScalarAsync<int>(query, new
            {
                idevento = funcion.evento!.idEvento,
                fecha = funcion.Fecha,
                estado = funcion.Estado.ToString()
            });

            return id;
        }

        public async Task<IEnumerable<Tarifa>> ObtenerTarifasDeFuncion(int id)
        {
            var db = _ado.GetDbConnection();
            var query = "SELECT * FROM Tarifa WHERE idFuncion = @idfuncion";

            return await db.QueryAsync<Tarifa>(query, new { idfuncion = id });
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
                fecha = funcion.Fecha,
                idfuncion = funcion.idFuncion
            });

            return rows > 0;
        }

        public async Task<string> CancelarFuncion(int idFuncion)
        {
            var db = _ado.GetDbConnection();

            // Verificar si la función existe
            var funcion = await db.QueryFirstOrDefaultAsync<Funcion>(
                "SELECT * FROM Funcion WHERE idFuncion = @idFuncion",
                new { idFuncion }
            );

            if (funcion == null)
                return "Función no encontrada";

            try
            {
                // Obtener entradas de la función
                var entradas = await db.QueryAsync<Entrada>(
                    "SELECT * FROM Entrada WHERE idFuncion = @idFuncion",
                    new { idFuncion }
                );

                foreach (var entrada in entradas)
                {
                    // Liberar stock
                    await db.ExecuteAsync(
                        "UPDATE Tarifa SET Stock = Stock + 1 WHERE idTarifa = @idTarifa",
                        new { idTarifa = entrada.tarifa.idTarifa }
                    );

                    // Anular entrada
                    await db.ExecuteAsync(
                        "UPDATE Entrada SET Estado = 'Anulada' WHERE idEntrada = @idEntrada",
                        new { idEntrada = entrada.idEntrada }
                    );
                }

                // Marcar la función como cancelada
                await db.ExecuteAsync(
                    "UPDATE Funcion SET Estado = 'Cancelada' WHERE idFuncion = @idFuncion",
                    new { idFuncion }
                );

                return string.Empty; // OK
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        public async Task<EEstados> ObtenerEstadoFuncion(string estadoFuncion)
        {
            var db = _ado.GetDbConnection();
            string query = "SELECT Estado FROM Funcion WHERE Estado = @estado";

            return await db.QueryFirstOrDefaultAsync<EEstados>(query, new
            {
                estado = estadoFuncion
            });
        }
    }
}