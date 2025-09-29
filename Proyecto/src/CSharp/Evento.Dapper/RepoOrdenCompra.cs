using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Evento.Core.Entidades;
using Evento.Core.Services.Enums;
using Evento.Core.Services.Repo;

namespace Evento.Dapper
{
    public class RepoOrdenCompra : IRepoOrdenCompra
    {
        private readonly IAdo _ado;

        public RepoOrdenCompra(IAdo ado)
        {
            _ado = ado;
        }

        public async Task<int> InsertOrdenCompra(OrdenesCompra orden)
        {
            if (orden.usuario == null) throw new Exception("El usuario es obligatorio");

            using var db = _ado.GetDbConnection();
            var sql = @"INSERT INTO OrdenesCompra(idUsuario, Fecha, Total, MetodoPago, Estado)
                        VALUES(@IdUsuario, @fecha, @total, @metodopago, @estado);
                        SELECT LAST_INSERT_ID();";
            var idOrden = await db.ExecuteScalarAsync<int>(sql, new
            {
                fecha = orden.Fecha,
                total = orden.Total,
                metodopago = orden.metodoPago.ToString(),
                estado = EEstados.Creado.ToString()
            });

            foreach (var entrada in orden.entradas)
            {
                string sqlReserva = @"INSERT INTO StockReservations(IdTarifa, Cantidad, ExpiraEn, IdOrdenCompra)
                          VALUES(@idtarifa, 1, @expiraEn, @idorden)";

                await db.ExecuteAsync(sqlReserva, new
                {
                    idtarifa = entrada.tarifa.idTarifa,
                    expiraEn = DateTime.UtcNow.AddMinutes(15),
                    idorden = idOrden
                });
            }
            return idOrden > 0 ? idOrden : 0;
        }
        public async Task<bool> UpdateOrdenCompra(OrdenesCompra orden)
        {
            if (orden.usuario == null) throw new Exception("El usuario es obligatorio");

            using var db = _ado.GetDbConnection();
            var sql = @"UPDATE OrdenesCompra 
                        SET idUsuario = @IdUsuario, Fecha = @fecha, Total = @total, MetodoPago = @metodopago, Estado = @estado
                        WHERE idOrdenCompra = @idordencompra";
            var rows = await db.ExecuteAsync(sql, new
            {
                IdUsuario = orden.usuario.idUsuario,
                fecha = orden.Fecha,
                total = orden.Total,
                metodopago = orden.metodoPago,
                estado = orden.Estado.ToString(),
                idordencompra = orden.idOrdenCompra
            });
            return rows > 0;
        }

        public async Task<bool> DeleteOrdenCompra(int id)
        {
            using var db = _ado.GetDbConnection();
            var rows = await db.ExecuteAsync("DELETE FROM OrdenesCompra WHERE idOrdenCompra = @Id", new { Id = id });
            return rows > 0;
        }

        public async Task<OrdenesCompra?> ObtenerOrdenCompra(int id)
        {
            var db = _ado.GetDbConnection();
            string sql = @"SELECT o.*, u.idUsuario, u.Apodo, u.DNI
                           FROM OrdenesCompra o
                           INNER JOIN Usuario u ON o.idUsuario = u.idUsuario
                           WHERE o.idOrdenCompra = @Id";

            var orden = (await db.QueryAsync<OrdenesCompra, Usuario, OrdenesCompra>(sql,
                        (o, u) => { o.usuario = u; return o; },
                        new { Id = id },
                        splitOn: "idUsuario"
                    )).SingleOrDefault();

            // Cargar lista de entradas
            if (orden != null)
            {
                orden.entradas = (await db.QueryAsync<Entrada>(
                    "SELECT * FROM Entrada WHERE idOrdenCompra = @Id",
                    new { Id = id }
                )).ToList();
            }

            return orden;
        }

        public async Task<IEnumerable<OrdenesCompra>> ObtenerOrdenesCompra()
        {
            var db = _ado.GetDbConnection();
            string sql = @"SELECT o.*, u.idUsuario, u.Apodo, u.DNI
                           FROM OrdenesCompra o
                           INNER JOIN Usuario u ON o.idUsuario = u.idUsuario";

            var ordenes = await db.QueryAsync<OrdenesCompra, Usuario, OrdenesCompra>(
                sql,
                (orden, usuario) => { orden.usuario = usuario; return orden; },
                splitOn: "idUsuario"
            );

            // Cargar entradas de cada orden
            foreach (var orden in ordenes)
            {
                orden.entradas = (await db.QueryAsync<Entrada>(
                    "SELECT * FROM Entrada WHERE idOrdenCompra = @Id",
                    new { Id = orden.idOrdenCompra }
                )).ToList();
            }

            return ordenes;
        }

        public async Task<IEnumerable<Entrada>> ObtenerEntradasPorOrden(int idOrdenCompra)
        {
            using var db = _ado.GetDbConnection();
            return await db.QueryAsync<Entrada>(
                "SELECT * FROM Entradas WHERE idOrdenCompra = @IdOrdenCompra",
                new { IdOrdenCompra = idOrdenCompra });
        }

        public async Task<string> PagarOrdenCompra(int idOrdenCompra)
        {
            var db = _ado.GetDbConnection();
            await ValidarOrdenParaPago(db, idOrdenCompra);

            var orden = await ObtenerOrdenCompra(idOrdenCompra);
            if (orden == null) return "Orden no encontrada";
            if (orden.Estado == EEstados.Pagado) return "Orden ya fue pagada";
            var entradas = await db.QueryAsync<Entrada>(
                "SELECT * FROM Entrada WHERE idOrdenCompra = @Id", new { Id = idOrdenCompra }
                );

            await ValidarStockEntradas(db, entradas);

            try
            {
                // Obtener reservas activas
                var reservas = await db.QueryAsync<StockReservaciones>(
                    "SELECT * FROM StockReservations WHERE IdOrdenCompra = @idOrden",
                    new { idOrden = idOrdenCompra }
                );

                foreach (var reserva in reservas)
                {
                    var tarifa = await db.QuerySingleAsync<Tarifa>(
                        "SELECT * FROM Tarifa WHERE idTarifa = @Id",
                        new { Id = reserva.idTarifa }
                    );

                    if (tarifa.Stock <= 0)
                        throw new Exception($"No hay stock suficiente para la tarifa {tarifa.Tipo}");

                    // Reducir stock real
                    if (tarifa.Stock <= 0)
                    {
                        return $"No hay suficiente stock para la tarifa: {tarifa.Tipo}";
                    }
                    await db.ExecuteAsync(
                            "UPDATE Tarifa SET Stock = Stock - 1 WHERE idTarifa = @Id AND Stock > 0",
                            new { Id = tarifa.idTarifa }
                            );

                    // Crear entrada definitiva con QR y precio pagado
                    string qr = Guid.NewGuid().ToString();
                    await db.ExecuteAsync(
                        "INSERT INTO Entrada (idTarifa, idOrdenCompra, PrecioPagado, Estado) VALUES (@idtarifa, @idOrden, @precio, @estado)",
                        new { idtarifa = tarifa.idTarifa, idOrden = idOrdenCompra, precio = tarifa.Precio, estado = tarifa.Estado }
                    );
                }
                foreach (var entrada in entradas)
                {
                    await db.ExecuteAsync("UPDATE Entrada SET PrecioPagado = @precio WHERE idEntrada = @Id",new
                    {
                        precio = entrada.tarifa.Precio,
                        Id = entrada.idEntrada
                    }
                    );
                }
                // Actualizar estado de la orden
                await db.ExecuteAsync(
                    "UPDATE OrdenesCompra SET Estado='Pagada' WHERE idOrdenCompra=@Id",
                    new { Id = idOrdenCompra }
                );
                return string.Empty; // OK
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public async Task<string> CancelarOrdenCompra(int idOrdenCompra)
        {
            using var db = _ado.GetDbConnection();
            var orden = await ObtenerOrdenCompra(idOrdenCompra);

            if (orden == null)
                throw new Exception("La orden no existe");

            if (orden.Estado == EEstados.Cancelado)
                throw new Exception("La orden ya está cancelada");

            try
            {
                // Obtener todas las entradas de la orden
                var entradas = await db.QueryAsync<Entrada>(
                    "SELECT * FROM Entrada WHERE idOrdenCompra = @Id",
                    new { Id = idOrdenCompra }
                );

                // Si la orden estaba pagada, devolver stock y anular entradas
                if (orden.Estado == EEstados.Pagado)
                {
                    foreach (var entrada in entradas)
                    {
                        await db.ExecuteAsync(
                            "UPDATE Tarifa SET Stock = Stock + 1 WHERE idTarifa = @Id",
                            new { Id = entrada.tarifa.idTarifa }
                        );

                        await db.ExecuteAsync(
                            "UPDATE Entrada SET Estado = 'Anulada' WHERE idEntrada = @Id",
                            new { Id = entrada.idEntrada }
                        );
                    }
                }

                // Actualizar estado de la orden
                await db.ExecuteAsync(
                    "UPDATE OrdenesCompra SET estado='Cancelada' WHERE idOrdenCompra=@Id",
                    new { Id = idOrdenCompra }
                );

                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<int> LiberarStockExpirado()
        {
            var db = _ado.GetDbConnection();

            // 1. Obtener entradas con stock reservado expiradas
            var entradasExpiradas = await db.QueryAsync<Entrada>(
                @"SELECT e.idEntrada, e.idTarifa
                FROM Entrada e
                WHERE e.EstadoQR IS NULL AND e.FechaReserva < NOW()"
            );

            int totalLiberadas = 0;

            foreach (var entrada in entradasExpiradas)
            {
                // 2. Incrementar stock de la tarifa correspondiente
                await db.ExecuteAsync(
                    @"UPDATE Tarifa 
                    SET Stock = Stock + 1 
                    WHERE idTarifa = @IdTarifa",
                    new { IdTarifa = entrada.tarifa.idTarifa }
                );

                // 3. Eliminar o marcar la reserva como liberada
                await db.ExecuteAsync(
                    @"UPDATE Entrada 
                    SET Estado = 'Expirada' 
                    WHERE idEntrada = @IdEntrada",
                    new { IdEntrada = entrada.idEntrada }
                );

                totalLiberadas++;
            }

            return totalLiberadas;
        }

        private async Task ValidarOrdenParaPago(IDbConnection db, int id)
        {
            // 1. Obtener la orden
            var orden = await ObtenerOrdenCompra(id);
            if (orden == null)
                throw new Exception("Orden no encontrada.");

            // 2. Verificar estado
            if (orden.Estado == EEstados.Pagado)
                throw new Exception("La orden ya fue pagada.");
            if (orden.Estado == EEstados.Cancelado)
                throw new Exception("La orden fue cancelada y no puede pagarse.");

            // 3. Verificar entradas asociadas
            var entradas = await db.QueryAsync<Entrada>(
                "SELECT * FROM Entrada WHERE idOrdenCompra = @Id",
                new { Id = id }
            );

            if (!entradas.Any())
                throw new Exception("La orden no tiene entradas asociadas para pagar.");
        }
        private async Task ValidarStockEntradas(IDbConnection db, IEnumerable<Entrada> entradas)
        {
            foreach (var entrada in entradas)
            {
                // Obtener la tarifa de la entrada
                var tarifa = await db.QueryFirstOrDefaultAsync<Tarifa>(
                    "SELECT * FROM Tarifa WHERE idTarifa = @Id",
                    new { Id = entrada.tarifa.idTarifa }
                );

                if (tarifa == null)
                    throw new Exception($"No se encontró la tarifa para la entrada {entrada.idEntrada}.");

                if (tarifa.Stock <= 0)
                    throw new Exception($"No hay stock suficiente para la tarifa {tarifa.Tipo} (Entrada {entrada.idEntrada}).");
            }
        }
    }
}