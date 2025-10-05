using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Evento.Core.Entidades;
using Evento.Core.Services.Enums;
using Evento.Core.Services.Repo;
using Evento.Core.Services.Utility;
using Org.BouncyCastle.Crypto.Digests;

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
            db.Open();
            using var tran = db.BeginTransaction();

            try
            {
                var sqlOrden = @"INSERT INTO OrdenesCompra(idUsuario, Fecha, Total, metodoPago, Estado)
                                 VALUES(@idUsuario, @fecha, @total, @metodopago, @estado);
                                 SELECT LAST_INSERT_ID();";
                var idOrden = await db.ExecuteScalarAsync<int>(sqlOrden, new
                {
                    idUsuario = orden.usuario.idUsuario,
                    fecha = orden.Fecha,
                    total = orden.Total,
                    metodopago = UniqueFormatStrings.NormalizarString(orden.metodoPago.ToString()),
                    estado = UniqueFormatStrings.NormalizarString(EEstados.Creado.ToString())
                }, tran);

                
                foreach (var entrada in orden.entradas)
                {
                    var sqlReserva = @"INSERT INTO StockReservaciones(idTarifa, Cantidad, ExpiraEn, idOrdenCompra)
                                       VALUES(@idTarifa, 1, @expiraEn, @idOrden)";
                    await db.ExecuteAsync(sqlReserva, new
                    {
                        idTarifa = entrada.tarifa.idTarifa,
                        expiraEn = DateTime.UtcNow.AddMinutes(15),
                        idOrden = idOrden
                    }, tran);
                }

                tran.Commit();
                return idOrden;
            }
            catch
            {
                tran.Rollback();
                throw;
            }
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
            using var db = _ado.GetDbConnection();
            string sql = @"SELECT o.*, u.idUsuario, u.Apodo, u.DNI
                           FROM OrdenesCompra o
                           INNER JOIN Usuario u ON o.idUsuario = u.idUsuario
                           WHERE o.idOrdenCompra = @Id";

            var orden = (await db.QueryAsync<OrdenesCompra, Usuario, OrdenesCompra>(
                sql,
                (o, u) =>
                {
                    o.usuario = u ?? throw new Exception("no existe el usuario");
                    return o;
                },
                new { Id = id },
                splitOn: "idUsuario"
            )).SingleOrDefault();

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
            using var db = _ado.GetDbConnection();
            string sql = @"SELECT o.*, u.idUsuario, u.Apodo, u.DNI
                           FROM OrdenesCompra o
                           INNER JOIN Usuario u ON o.idUsuario = u.idUsuario";

            var ordenes = await db.QueryAsync<OrdenesCompra?, Usuario?, OrdenesCompra?>(
                sql,
                (orden, usuario) => { orden.usuario = usuario; return orden; },
                splitOn: "idUsuario"
            );
            if (ordenes.Any())
            {
                return ordenes;
            }
            
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
            using var db = _ado.GetDbConnection();
            db.Open();
            using var transaction = db.BeginTransaction();

            try
            {
                var orden = await ObtenerOrdenCompra(idOrdenCompra);
                if (orden == null) return "Orden no encontrada";

                if (UniqueFormatStrings.NormalizarString(orden.Estado.ToString()) == 
                    UniqueFormatStrings.NormalizarString(EEstados.Pagado.ToString()))
                    return "Orden ya fue pagada";

                if (UniqueFormatStrings.NormalizarString(orden.Estado.ToString()) == 
                    UniqueFormatStrings.NormalizarString(EEstados.Cancelado.ToString()))
                    return "Orden fue cancelada y no puede pagarse";

                
                var entradas = await db.QueryAsync<Entrada>(
                    "SELECT * FROM Entrada WHERE idOrdenCompra = @Id AND Estado = 'Creada'",
                    new { Id = idOrdenCompra }, transaction);

                foreach (var entrada in entradas)
                {
                   
                    var tarifa = await db.QueryFirstOrDefaultAsync<Tarifa>(
                        "SELECT * FROM Tarifa WHERE idTarifa = @Id",
                        new { Id = entrada.tarifa.idTarifa }, transaction);

                    if (tarifa == null) throw new Exception($"Tarifa no encontrada para entrada {entrada.idEntrada}");
                    if (tarifa.Stock <= 0) throw new Exception($"No hay stock suficiente para la tarifa {tarifa.Tipo}");

                    // Reducir stock
                    await db.ExecuteAsync(
                        "UPDATE Tarifa SET Stock = Stock - 1 WHERE idTarifa = @Id AND Stock > 0",
                        new { Id = tarifa.idTarifa }, transaction);

                    
                    await db.ExecuteAsync(
                        @"UPDATE Entrada SET Estado = 'Pagado', PrecioPagado = @precio 
                        WHERE idEntrada = @IdEntrada",
                        new { precio = tarifa.Precio, IdEntrada = entrada.idEntrada }, transaction);
                }

                
                await db.ExecuteAsync(
                    "UPDATE OrdenesCompra SET Estado = 'Pagada' WHERE idOrdenCompra = @Id",
                    new { Id = idOrdenCompra }, transaction);

                transaction.Commit();
                return string.Empty; // OK
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return ex.Message;
            }
        }
        public async Task<string> CancelarOrdenCompra(int idOrdenCompra)
        {
            using var db = _ado.GetDbConnection();
            db.Open();
            using var transaction = db.BeginTransaction();

            try
            {
                var orden = await ObtenerOrdenCompra(idOrdenCompra);
                if (orden == null) return "Orden no encontrada";

                if (UniqueFormatStrings.NormalizarString(orden.Estado.ToString()) == 
                    UniqueFormatStrings.NormalizarString(EEstados.Cancelado.ToString()))
                    return "Orden ya está cancelada";

                
                var entradas = await db.QueryAsync<Entrada>(
                    "SELECT * FROM Entrada WHERE idOrdenCompra = @Id",
                    new { Id = idOrdenCompra }, transaction);

                
                if (UniqueFormatStrings.NormalizarString(orden.Estado.ToString()) ==
                    UniqueFormatStrings.NormalizarString(EEstados.Pagado.ToString()))
                {
                    foreach (var entrada in entradas)
                    {
                        await db.ExecuteAsync(
                            "UPDATE Tarifa SET Stock = Stock + 1 WHERE idTarifa = @IdTarifa",
                            new { IdTarifa = entrada.tarifa.idTarifa }, transaction);

                        await db.ExecuteAsync(
                            "UPDATE Entrada SET Estado = 'Anulada' WHERE idEntrada = @IdEntrada",
                            new { IdEntrada = entrada.idEntrada }, transaction);
                    }
                }

                
                await db.ExecuteAsync(
                    "UPDATE OrdenesCompra SET Estado = 'Cancelada' WHERE idOrdenCompra = @Id",
                    new { Id = idOrdenCompra }, transaction);

                transaction.Commit();
                return string.Empty; // OK
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return ex.Message;
            }
        }

        public async Task<int> LiberarStockExpirado()
        {
            using var db = _ado.GetDbConnection();
            using var tran = db.BeginTransaction();

            var expiradas = await db.QueryAsync<StockReservaciones>(
                "SELECT * FROM StockReservaciones WHERE ExpiraEn < NOW()"
            );

            int total = 0;
            foreach (var res in expiradas)
            {
                await db.ExecuteAsync(
                    "UPDATE Tarifa SET Stock = Stock + @cantidad WHERE idTarifa = @idTarifa",
                    new { cantidad = res.Cantidad, idTarifa = res.idTarifa }, tran
                );

                await db.ExecuteAsync(
                    "DELETE FROM StockReservaciones WHERE IdStockReservacion = @id",
                    new { id = res.idStockReservacion }, tran
                );

                total++;
            }
            tran.Commit();
            
            return total;
        }

        private async Task ValidarOrdenParaPago(IDbConnection db, int id)
        {
            // 1. Obtener la orden
            var orden = await ObtenerOrdenCompra(id);
            if (orden == null)
                throw new Exception("Orden no encontrada.");

            // 2. Verificar estado
            if (UniqueFormatStrings.NormalizarString(orden.Estado.ToString()) == UniqueFormatStrings.NormalizarString(EEstados.Pagado.ToString()))
                throw new Exception("La orden ya fue pagada.");
            if (UniqueFormatStrings.NormalizarString(orden.Estado.ToString()) == UniqueFormatStrings.NormalizarString(EEstados.Cancelado.ToString()))
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

        public EMetodoPago ObtenerMetodoPago(string metodo)
        {
            string metodoPagoNormalizado = UniqueFormatStrings.NormalizarString(metodo);

            foreach (var nombre in Enum.GetNames(typeof(EMetodoPago)))
            {
                if (nombre.ToLowerInvariant() == metodoPagoNormalizado)
                    return (EMetodoPago)Enum.Parse(typeof(EMetodoPago), nombre);
            }
            throw new ArgumentException($"El metodo de pago: {metodoPagoNormalizado} no es valido");
        }

        public EEstados ObtenerEstado(string estadoOC)
        {
            string estadoNormalizado = UniqueFormatStrings.NormalizarString(estadoOC);

            foreach (var nombre in Enum.GetNames(typeof(EEstados)))
            {
                if (nombre.ToLowerInvariant() == estadoNormalizado)
                    return (EEstados)Enum.Parse(typeof(EEstados), nombre);
            }
            throw new ArgumentException($"El estado: {estadoNormalizado} no es valido");
        }
    }
}