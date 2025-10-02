using Dapper;
using Evento.Core.Entidades;
using Evento.Core.Services.Repo;
using Evento.Core.Services.Enums;
using Evento.Core.Services.Utility;
using Evento.Core.DTOs;

namespace Evento.Dapper
{
    public class RepoEvento : IRepoEvento
    {
        private readonly IAdo _ado;

        public RepoEvento(IAdo ado) => _ado = ado;

        public async Task<string> CancelarEvento(int idEvento)
        {
            var db = _ado.GetDbConnection();
            var evento = await db.QueryFirstOrDefaultAsync<Eventos>(
                "SELECT * FROM Eventos WHERE idEvento = @Id",
                new { Id = idEvento }
            );
            if (evento == null) return "Evento no encontrado";

            try
            {
                // Obtener funciones del evento
                var funciones = await db.QueryAsync<Funcion>(
                    "SELECT * FROM Funcion WHERE idEvento = @Id",
                    new { Id = idEvento }
                );

                foreach (var funcion in funciones)
                {
                    // Obtener entradas de la función
                    var entradas = await db.QueryAsync<Entrada>(
                        "SELECT * FROM Entrada WHERE idFuncion = @Id",
                        new { Id = funcion.idFuncion }
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
                }

                // Actualizar estado del evento
                await db.ExecuteAsync(
                    "UPDATE Eventos SET estado = 'Cancelado' WHERE idEvento = @Id",
                    new { Id = idEvento }
                );

                return string.Empty; // OK
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<bool> DeleteEvento(int id)
        {
            var db = _ado.GetDbConnection();
            var rows = await db.ExecuteAsync("DELETE FROM Evento WHERE idEvento = @Id", new { Id = id });
            return rows > 0;
        }

        public async Task<int> InsertEvento(Eventos evento)
        {
            var db = _ado.GetDbConnection();
            var rows = await db.ExecuteAsync("INSERT INTO Evento(idEvento, Nombre, idTipoEvento, fechaInicio, fechaFin, Estado) VALUES(@idevento, @nombre, @tipoevento, @fechainicio, @fechafin, @estado)", new
            {
                idevento = evento.idEvento,
                nombre = evento.Nombre,
                tipoevento = evento.tipoEvento.idTipoEvento,
                fechainicio = evento.fechaInicio,
                fechafin = evento.fechaFin,
                estado = evento.EstadoEvento
            });
            return rows > 0 ? rows : 0;
        }

        public async Task<Eventos?> ObtenerEventoPorId(int id)
        {
            var db = _ado.GetDbConnection();
            return await db.QueryFirstOrDefaultAsync<Eventos?>("SELECT * FROM Evento WHERE idEvento = @idevento", new { idevento = id });
        }

        public async Task<Eventos?> ObtenerEventoPorNombre(string nombre)
        {
            var db = _ado.GetDbConnection();
            var query = "SELECT * FROM Evento WHERE Nombre = @name";

            return await db.QueryFirstAsync<Eventos>(query, new { name = nombre });
        }

        public async Task<IEnumerable<Funcion>> ObtenerFuncionesPorEventoAsync(int idEvento)
        {
            var db = _ado.GetDbConnection();
            return await db.QueryAsync<Funcion>("SELECT * FROM Funcion WHERE idEvento = @idevento", new { idevento = idEvento });
        }

        public async Task<TipoEventoDto?> ObtenerTipoEventoPorNombre(string tipo)
        {
            var db = _ado.GetDbConnection();
            string query = "SELECT * FROM TipoEvento WHERE LOWER(tipoEvento) = @tipoevento";

            var tipoevento = await db.QueryFirstAsync<TipoEvento?>(query, new { tipoevento = tipo });
            if (tipoevento == null) return null;

            return new TipoEventoDto
            {
                idTipoEvento = tipoevento.idTipoEvento,
                tipoEvento = Enum.Parse<ETipoEvento>(tipoevento.tipoEvento, true)
            };
        }
        public async Task<IEnumerable<Eventos>> ObtenerTodos()
        {
            var db = _ado.GetDbConnection();
            return await db.QueryAsync<Eventos>("SELECT * FROM Evento");
        }

        public async Task<string> PublicarEvento(int id)
        {
            using var db = _ado.GetDbConnection();

            var evento = await ObtenerEventoPorId(id);
            if (evento == null)
                throw new Exception("El evento no existe");

            if (evento.EstadoEvento.ToString() == UniqueFormatStrings.NormalizarString(EEstados.Publicado.ToString()))
                throw new Exception("El evento ya está publicado");

            string query = "SELECT * FROM Funcion f JOIN Tarifa t USING (idFuncion) WHERE f.idEvento = @idevento AND t.Stock > 0 LIMIT 1";
            var respuesta = await db.ExecuteAsync(query, new
            {
                idevento = id
            });
            if (respuesta < 0)
                throw new Exception("No se puede publicar el Evento por falta de Stock");

            var rows = await db.ExecuteAsync(
                "UPDATE Evento SET Estado = 'Publicado' WHERE idEvento = @IdEvento",
                new { IdEvento = id });

            return rows > 0 ? "Evento publicado correctamente" : "No se pudo publicar el evento";
        }

        public async Task<bool> UpdateEvento(Eventos evento)
        {
            var db = _ado.GetDbConnection();
            var query = "UPDATE Evento SET Nombre = @nombre, tipoEvento = @tipoevento, fechaInicio = @fechainicio, fechaFin = @fechafin WHERE idEvento = @idevento";
            var rows = await db.ExecuteAsync(query, new
            {
                idevento = evento.idEvento,
                nombre = evento.Nombre,
                tipoevento = evento.tipoEvento.tipoEvento,
                fechainicio = evento.fechaInicio,
                fechafin = evento.fechaFin
            });
            return rows > 0;
        }
    }
}