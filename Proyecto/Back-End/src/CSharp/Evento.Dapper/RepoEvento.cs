using Dapper;
using Evento.Core.Entidades;
using Evento.Core.Services.Repo;
using Evento.Core.Services.Enums;
using Evento.Core.Services.Utility;
using Evento.Core.DTOs;
using ZstdSharp;

namespace Evento.Dapper
{
    public class RepoEvento : IRepoEvento
    {
        private readonly IAdo _ado;

        public RepoEvento(IAdo ado) => _ado = ado;

        public async Task<string> CancelarEvento(int idEvento)
        {
            using var db = _ado.GetDbConnection();
            
            var evento = await ObtenerEventoPorId(idEvento);
            if (evento is null)
                throw new ArgumentNullException("Evento no encontrado");
            if (evento.EstadoEvento == EEstados.Cancelado)
                throw new Exception("Evento ya cancelado");
            try
            {
                var funciones = await db.QueryAsync<Funcion>(
                    "SELECT * FROM Funcion WHERE idEvento = @Id",
                    new { Id = idEvento }
                );
                if (!funciones.Any())
                    throw new ArgumentNullException ("El evento no tiene funciones");

                foreach (var funcion in funciones)
                    {
                        var entradas = await db.QueryAsync<Entrada, Tarifa, Entrada>(
                                @"SELECT e.idEntrada, e.idTarifa AS Entrada_Tarifa, e.idOrdenCompra, e.Estado AS Entrada_Estado, e.PrecioPagado, t.idTarifa AS Tarifa_idTarifa, t.idFuncion AS Funcion_idFuncion, t.Stock, t.Precio, t.Estado AS Tarifa_Estado, t.Tipo
                                FROM Entrada e
                                INNER JOIN Tarifa t ON e.idTarifa = t.idTarifa
                                WHERE t.idFuncion = @idFuncion",
                                (entrada, tarifa) =>
                                {
                                    entrada.tarifa = tarifa;
                                    return entrada;
                                },
                                new { idFuncion = funcion.idFuncion },
                                splitOn: "Tarifa_idTarifa"
                            );
                        if (!entradas.Any())
                            throw new ArgumentNullException("No hay entradas.");

                        foreach (var entrada in entradas)
                        {

                            await db.ExecuteAsync(
                                "UPDATE Tarifa SET Stock = Stock + 1 WHERE idTarifa = @Id",
                                new { Id = entrada.tarifa?.idTarifa! }
                            );

                            await db.ExecuteAsync(
                                "UPDATE Entrada SET Estado = @estado WHERE idEntrada = @Id",
                                new { Id = entrada.idEntrada, estado = EEstados.Cancelado.ToString() }
                            );
                        }
                    }


                await db.ExecuteAsync(
                    "UPDATE Evento SET Estado = @estado WHERE idEvento = @Id",
                    new { Id = idEvento, estado = EEstados.Cancelado.ToString() }
                );

                return string.Empty;
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
            var sql = @"INSERT INTO Evento(Nombre, idTipoEvento, Estado, fechaInicio, fechaFin)
                        VALUES(@Nombre, @idTipoEvento, @Estado, @fechaInicio, @fechaFin);
                        SELECT LAST_INSERT_ID();";

            var newId = await db.QuerySingleAsync<int>(sql, new
            {
                evento.Nombre,
                evento.idTipoEvento,
                Estado = evento.EstadoEvento.ToString(),
                evento.fechaInicio,
                evento.fechaFin
            });

            return newId;
        }

        public async Task<Eventos?> ObtenerEventoPorId(int id)
        {
            var db = _ado.GetDbConnection();
            string query = @"
                            SELECT e.idEvento, e.Nombre, e.idTipoEvento AS Evento_idTipoEvento, e.Estado AS EstadoEvento, e.fechaInicio, e.fechaFin,
                                t.idTipoEvento AS Tipo_idTipoEvento, t.tipoEvento
                            FROM Evento e
                            INNER JOIN TipoEvento t ON e.idTipoEvento = t.idTipoEvento
                            WHERE idEvento = @idevento
                            LIMIT 1";

            var evento = await db.QueryAsync<Eventos, TipoEvento, Eventos>(
                query,
                (ev, tipo) =>
                {
                    ev.tipoEvento = tipo;
                    if (Enum.TryParse<EEstados>(ev.EstadoEvento.ToString(), true, out var estado))
                        ev.EstadoEvento = estado;
                    else
                        ev.EstadoEvento = EEstados.Pendiente;
                    return ev;
                },
                new { idevento = id },
                splitOn: "Tipo_idTipoEvento"
            );
            var result = evento.FirstOrDefault();
            return result;
        }

        public async Task<Eventos?> ObtenerEventoPorNombre(string nombre)
        {
            var db = _ado.GetDbConnection();
             var sql = @"
                        SELECT e.idEvento, e.Nombre, e.idTipoEvento, e.fechaInicio, e.fechaFin, e.Estado,
                            t.idTipoEvento, t.tipoEvento
                        FROM Evento e
                        INNER JOIN TipoEvento t ON e.idTipoEvento = t.idTipoEvento
                        WHERE e.Nombre = @Nombre";

            var result = await db.QueryAsync<Eventos, TipoEvento, Eventos>(
                sql,
                (ev, tipo) =>
                {
                    ev.tipoEvento = tipo;
                    if (Enum.TryParse<EEstados>(ev.EstadoEvento.ToString(), true, out var estado))
                        ev.EstadoEvento = estado;
                    else
                        ev.EstadoEvento = EEstados.Pendiente;
                    return ev;
                },
                new { Nombre = nombre },
                splitOn: "idTipoEvento"
            );

            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<Funcion>> ObtenerFuncionesPorEvento(int idEvento)
        {
            var db = _ado.GetDbConnection();
            var sql = @"
                        SELECT f.idFuncion, f.idEvento, f.Estado, f.Fecha,
                            e.idEvento, e.Nombre, e.idTipoEvento, e.fechaInicio, e.fechaFin, e.Estado
                        FROM Funcion f
                        INNER JOIN Evento e ON f.idEvento = e.idEvento
                        WHERE f.idEvento = @idEvento";

            var result = await db.QueryAsync<Funcion, Eventos, Funcion>(
                sql,
                (func, evento) =>
                {
                    func.evento = evento;
                    return func;
                },
                new { idEvento },
                splitOn: "idEvento"
            );

            return result;
        }

        public async Task<TipoEventoDto?> ObtenerTipoEventoPorNombre(string tipo)
        {
            using var db = _ado.GetDbConnection();
            string query = $"SELECT * FROM TipoEvento WHERE LOWER(tipoEvento) = @tipo";
            var tipoEvento = await db.QueryFirstAsync<TipoEvento>(query, new
            {
                tipo = UniqueFormatStrings.NormalizarString(tipo)
            });
    
            return new TipoEventoDto
            {
                idTipoEvento = tipoEvento.idTipoEvento,
                tipoEvento = Enum.Parse<ETipoEvento>(tipoEvento.tipoEvento, true)
            };
        }
        public async Task<IEnumerable<Eventos>> ObtenerTodos()
        {
            var db = _ado.GetDbConnection();
            string query = @"
                            SELECT e.idEvento, e.Nombre, e.idTipoEvento AS Evento_idTipoEvento, e.Estado AS EstadoEvento, e.fechaInicio, e.fechaFin,
                                t.idTipoEvento AS Tipo_idTipoEvento, t.tipoEvento
                            FROM Evento e
                            INNER JOIN TipoEvento t ON e.idTipoEvento = t.idTipoEvento";

            var eventos = await db.QueryAsync<Eventos, TipoEvento, Eventos>(
                        query,
                        (ev, tipo) =>
                        {
                            ev.tipoEvento = tipo;

                            if (Enum.TryParse<EEstados>(ev.EstadoEvento.ToString(), true, out var estado))
                                ev.EstadoEvento = estado;
                            else
                                ev.EstadoEvento = EEstados.Pendiente;

                            return ev;
                        },
                        splitOn: "Tipo_idTipoEvento"
                    );
            return eventos;
        }

        public async Task<string> PublicarEvento(int id)
        {
            using var db = _ado.GetDbConnection();

            var evento = await ObtenerEventoPorId(id);
            if (evento == null)
                throw new Exception("El evento no existe");

            if (evento.EstadoEvento.ToString().ToLower() == UniqueFormatStrings.NormalizarString(EEstados.Publicado.ToString()))
                throw new Exception("El evento ya está publicado");

            string query = "SELECT * FROM Funcion f JOIN Tarifa t USING (idFuncion) WHERE f.idEvento = @idevento AND t.Stock > 0";
            var funcionesConTarifas = await db.QueryAsync<Funcion, Tarifa, FuncionTarifaDto>(
                query,
                (funcion, tarifa) => new FuncionTarifaDto
                {
                    funcion = funcion,
                    tarifa = tarifa
                },
                new { idEvento = id },
                splitOn: "idTarifa"
            );
            if (!funcionesConTarifas.Any())
                throw new Exception("No se puede publicar el evento por falta de stock");

            var rows = await db.ExecuteAsync(
                "UPDATE Evento SET Estado = @estado WHERE idEvento = @IdEvento",
                new { IdEvento = id, estado = UniqueFormatStrings.NormalizarString(EEstados.Publicado.ToString()) });

            if (rows > 0)
            {
                evento.EstadoEvento = EEstados.Publicado;
                return "Evento publicado correctamente";
            }
            throw new Exception("No se pudo publicar el evento");
        }

        public async Task<bool> UpdateEvento(Eventos evento)
        {
            var db = _ado.GetDbConnection();

            string query = @"
                            UPDATE Evento
                            SET Nombre = @Nombre,
                                idTipoEvento = @idTipoEvento,
                                Estado = @Estado,
                                fechaInicio = @fechaInicio,
                                fechaFin = @fechaFin
                            WHERE idEvento = @idEvento";

            var rows = await db.ExecuteAsync(query, new
            {
                evento.Nombre,
                evento.idTipoEvento,
                Estado = evento.EstadoEvento.ToString(),
                evento.fechaInicio,
                evento.fechaFin,
                evento.idEvento
            });

            return rows > 0;
        }
    }
}