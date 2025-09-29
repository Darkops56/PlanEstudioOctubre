using Dapper;
using Evento.Core.Entidades;
using Evento.Core.Services.Enums;
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
            var rows = await db.ExecuteAsync(query, new
            {
                idevento = funcion.evento.idEvento,
                fecha = funcion.Fecha
            });
            return rows > 0 ? rows : 0;
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
                "SELECT * FROM Funcion WHERE idFuncion = @Id",
                new { Id = idFuncion }
            );
            if (funcion == null) return "Función no encontrada";

            try
            {
                // Obtener entradas de la función
                var entradas = await db.QueryAsync<Entrada>(
                    "SELECT * FROM Entrada WHERE idFuncion = @Id",
                    new { Id = idFuncion }
                );

                foreach (var entrada in entradas)
                {
                    // Liberar stock
                    await db.ExecuteAsync(
                        "UPDATE Tarifa SET Stock = Stock + 1 WHERE idTarifa = @Id",
                        new { Id = entrada.tarifa.idTarifa }
                    );

                    // Anular entrada
                    await db.ExecuteAsync(
                        "UPDATE Entrada SET EstadoQR = 'Anulada' WHERE idEntrada = @Id",
                        new { Id = entrada.idEntrada }
                    );
                }

                // Opcional: marcar la función como cancelada si tienes un campo Estado
                await db.ExecuteAsync(
                    "UPDATE Funcion SET Estado = 'Cancelada' WHERE idFuncion = @Id",
                    new { Id = idFuncion }
                );

                return string.Empty; // OK
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}