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
                
            if (UniqueFormatStrings.NormalizarString(funcion.Estado.ToString()) == UniqueFormatStrings.NormalizarString(EEstados.Cancelado.ToString()))
                return "la funcion ya fue cancelada";

            db.Open();
            using var tran = db.BeginTransaction();
            try
            {
                var sql = @"
                    SELECT 
                        e.idEntrada, e.idTarifa, e.idOrdenCompra AS Entrada_idOrdenCompra, e.Estado AS EstadoEntrada, e.PrecioPagado,
                        t.idTarifa AS Tarifa_idTarifa, t.idFuncion AS Tarifa_idFuncion, t.Stock, t.Precio, t.Estado AS TarifaEstado, t.Tipo,
                        f.idFuncion AS Funcion_idFuncion, f.idEvento, f.Nombre AS FuncionNombre, f.Estado AS FuncionEstado, f.Fecha AS FuncionFecha,
                        o.idOrdenCompra AS OrdenCompra_idOrdenCompra, o.idUsuario, o.Fecha AS OrdenFecha, o.Total, o.metodoPago, o.estado AS OrdenEstado
                    FROM Entrada e
                    INNER JOIN Tarifa t ON e.idTarifa = t.idTarifa
                    INNER JOIN Funcion f ON t.idFuncion = f.idFuncion
                    INNER JOIN OrdenesCompra o ON e.idOrdenCompra = o.idOrdenCompra
                    WHERE f.idFuncion = @idFuncion";

                var entradas = await db.QueryAsync<Entrada, Tarifa, Funcion, OrdenesCompra, Entrada>(
                    sql,
                    (entrada, tarifa, funcionMap, orden) =>
                    {
                        tarifa.funcion = funcionMap;
                        entrada.tarifa = tarifa;
                        entrada.ordenesCompra = orden;

                        
                        if (Enum.TryParse<EEstados>(entrada.Estado.ToString(), true, out var estadoEntrada))
                            entrada.Estado = estadoEntrada;
                        else
                            entrada.Estado = EEstados.Pendiente;

                        if (Enum.TryParse<EEstados>(tarifa.Estado.ToString(), true, out var estadoTarifa))
                            tarifa.Estado = estadoTarifa;

                        if (Enum.TryParse<EEstados>(funcionMap.Estado.ToString(), true, out var estadoFuncion))
                            funcionMap.Estado = estadoFuncion;

                        if (Enum.TryParse<EEstados>(orden.Estado.ToString(), true, out var estadoOrden))
                            orden.Estado = estadoOrden;

                        return entrada;
                    },
                    new { idFuncion },
                    splitOn: "Tarifa_idTarifa,Funcion_idFuncion,OrdenCompra_idOrdenCompra",
                    transaction: tran
                );

                
                foreach (var entrada in entradas)
                {
                    await db.ExecuteAsync(
                        "UPDATE Tarifa SET Stock = Stock + 1 WHERE idTarifa = @idTarifa",
                        new { idTarifa = entrada.tarifa.idTarifa },
                        tran
                    );

                    await db.ExecuteAsync(
                        "UPDATE Entrada SET Estado = @estado WHERE idEntrada = @idEntrada",
                        new { idEntrada = entrada.idEntrada, estado = EEstados.Anulada.ToString() },
                        tran
                    );
                }

                await db.ExecuteAsync(
                    "UPDATE Funcion SET Estado = @estado WHERE idFuncion = @idFuncion",
                    new { idFuncion, estado = EEstados.Cancelado.ToString() },
                    tran
                );

                tran.Commit();
                return "Se canceló correctamente";
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