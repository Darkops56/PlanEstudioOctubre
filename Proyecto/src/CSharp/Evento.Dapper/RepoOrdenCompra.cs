using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Evento.Core.Entidades;
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

        public async Task<string> CancelarOrdenCompra(int id)
        {
            var db = _ado.GetDbConnection();
            var orden = await ObtenerOrdenCompra(id);
            if (orden == null) return "Orden no encontrada";

            if (orden.estado == "Cancelada")
                return "Orden ya fue cancelada";

            using var transaction = db.BeginTransaction();

            try
            {
                // 1. Obtener todas las entradas de la orden
                var entradas = await db.QueryAsync<Entrada>(
                    "SELECT * FROM Entrada WHERE idOrdenCompra = @Id",
                    new { Id = id },
                    transaction: transaction
                );

                // 2. Si la orden estaba pagada, devolver stock de las tarifas
                if (orden.estado == "Pagada")
                {
                    foreach (var entrada in entradas)
                    {
                        await db.ExecuteAsync(
                            "UPDATE Tarifa SET Stock = Stock + 1 WHERE idTarifa = @Id",
                            new { Id = entrada.tarifa.idTarifa },
                            transaction: transaction
                        );

                        // 3. Anular entrada
                        await db.ExecuteAsync(
                            "UPDATE Entrada SET EstadoQR = 'Anulada' WHERE idEntrada = @Id",
                            new { Id = entrada.idEntrada },
                            transaction: transaction
                        );
                    }
                }

                // 4. Actualizar estado de la orden
                await db.ExecuteAsync(
                    "UPDATE OrdenesCompra SET estado='Cancelada' WHERE idOrdenCompra=@Id",
                    new { Id = id },
                    transaction: transaction
                );

                transaction.Commit();
                return string.Empty; // OK
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return ex.Message;
            }
        }

        public async Task<int> InsertOrdenCompra(OrdenesCompra ordenesCompra)
        {
            var db = _ado.GetDbConnection();
            string sql = @"INSERT INTO OrdenesCompra 
                           (Fecha, Total, metodoPago, estado)
                           VALUES (@Fecha, @Total, @MetodoPago, @Estado)";

            var parametros = new
            {
                Fecha = ordenesCompra.Fecha,
                Total = ordenesCompra.Total,
                MetodoPago = ordenesCompra.metodoPago.ToString(),
                Estado = "Creada"
            };

            int rows = await db.ExecuteAsync(sql, parametros);
            return rows > 0 ? rows : 0;
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
            return ordenes;
        }

        public async Task<string> PagarOrdenCompra(int id)
        {
            var db = _ado.GetDbConnection();
            var orden = await ObtenerOrdenCompra(id);
            if (orden == null) return "Orden no encontrada";
            if (orden.estado == "Pagada") return "Orden ya fue pagada";
            using var transaction = db.BeginTransaction();

            try
            {
                // 1. Obtener las entradas asociadas a la orden
                var entradas = await db.QueryAsync<Entrada>(
                    "SELECT * FROM Entrada WHERE idOrdenCompra = @Id",
                    new { Id = id },
                    transaction: transaction
                );

                // 2. Verificar stock de cada tarifa
                foreach (var entrada in entradas)
                {
                    var tarifa = await db.QuerySingleAsync<Tarifa>(
                        "SELECT * FROM Tarifa WHERE idTarifa = @Id",
                        new { Id = entrada.tarifa.idTarifa },
                        transaction: transaction
                    );

                    if (tarifa.Stock <= 0)
                        throw new Exception($"No hay stock suficiente para la tarifa {tarifa.Tipo}");

                    // 3. Reducir stock
                    await db.ExecuteAsync(
                        "UPDATE Tarifa SET Stock = Stock - 1 WHERE idTarifa = @Id",
                        new { Id = tarifa.idTarifa },
                        transaction: transaction
                    );

                    // 4. Actualizar precio pagado en la entrada
                    await db.ExecuteAsync(
                        "UPDATE Entrada SET PrecioPagado = @precio WHERE idEntrada = @Id",
                        new { precio = tarifa.Precio, Id = entrada.idEntrada },
                        transaction: transaction
                    );
                }

                // 5. Actualizar estado de la orden
                await db.ExecuteAsync(
                    "UPDATE OrdenesCompra SET estado='Pagada' WHERE idOrdenCompra=@Id",
                    new { Id = id },
                    transaction: transaction
                );

                transaction.Commit();
                return string.Empty; // OK
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return ex.Message;
            }
        }
    }
}