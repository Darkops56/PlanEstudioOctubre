using MySql.Data.MySqlClient;
using Dapper;
using Evento.Core.Entidades;
using Evento.Core.Services.Repo;
using Evento.Core.Services.Enums;
using Evento.Core.Services.Utility;

namespace Evento.Dapper
{
    public class RepoEntrada : IRepoEntrada
    {
        private readonly IAdo _ado;
        public RepoEntrada(IAdo ado) => _ado = ado;

        public async Task<string> AnularEntrada(int id)
        {
            using var db = _ado.GetDbConnection();

            var entrada = await ObtenerEntrada(id);
            if (entrada == null)
                throw new Exception("La entrada no existe");

            if (entrada.Estado.ToString().ToLower().Trim() == UniqueFormatStrings.NormalizarString(EEstados.Cancelado.ToString()))
                throw new Exception("La entrada ya está anulada");
            db.Open();
            using var tran = db.BeginTransaction();

            try
            {
                
                await db.ExecuteAsync(
                    "UPDATE Entrada SET Estado = 'anulada' WHERE idEntrada = @IdEntrada",
                    new { IdEntrada = id },
                    tran
                );

                
                if (entrada.Estado.ToString().ToLower().Trim() == UniqueFormatStrings.NormalizarString(EEstados.Pagado.ToString()))
                {
                    await db.ExecuteAsync(
                        "UPDATE Tarifa SET Stock = Stock + 1 WHERE idTarifa = @idTarifa",
                        new { idTarifa = entrada.tarifa.idTarifa },
                        tran
                    );
                }

                tran.Commit();
                return string.Empty;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                return ex.Message;
            }
        }

        public async Task<bool> DeleteEntrada(int id)
        {
            using var db = _ado.GetDbConnection();
            var rows = await db.ExecuteAsync("DELETE FROM Entrada WHERE idEntrada = @Id", new { Id = id });
            return rows > 0;
        }

        public async Task<int> InsertEntrada(Entrada entrada)
        {
            if (entrada.tarifa?.idTarifa == null)
                throw new ArgumentNullException("Tarifa no puede estar vacio");
            if (entrada.ordenesCompra?.idOrdenCompra == null)
                throw new ArgumentNullException("OrdenCompra no puede estar vacio");
            if (entrada.tarifa.Stock == 0)
                throw new ArgumentOutOfRangeException("No se puede crear una entrada con una tarifa sin stock");

            using var db = _ado.GetDbConnection();
            db.Open();
            var trans = db.BeginTransaction();

            var idEntrada = await db.ExecuteScalarAsync<int>(
                "INSERT INTO Entrada(idTarifa, idOrdenCompra, Estado, PrecioPagado) " +
                "VALUES(@idtarifa, @idordencompra, @estado, @preciopagado); SELECT LAST_INSERT_ID();",
                new
                {
                    idtarifa = entrada.tarifa.idTarifa,
                    idordencompra = entrada.ordenesCompra.idOrdenCompra,
                    estado = entrada.Estado.ToString(),
                    preciopagado = entrada.PrecioPagado
                },
                trans
            );

            if (entrada.Estado == EEstados.Pagado)
            {
                var token = Core.Services.QrHelper.GenerarToken(10);
                string qrUrl = Core.Services.QrHelper.GenerarUrlValidacion(idEntrada, token);

                var qr = new QR
                {
                    idEntrada = idEntrada,
                    url = qrUrl,
                    token = token,
                    ExpiraEn = DateTime.Now.AddMinutes(20),
                    VCard = "",
                    Estado = EEstados.Activo
                };

                var repoQR = new RepoQR(_ado);
                await repoQR.InsertQR(qr, db, trans);
                return 1;
            }
            trans.Commit();
            return idEntrada;
        }

        public async Task<bool> MarcarEntradaUsada(int id)
        {
            using var db = _ado.GetDbConnection();
        var filas = await db.ExecuteAsync(
            "UPDATE Entrada SET Estado = @estado WHERE idEntrada = @idEntrada",
            new { estado = "Usado", idEntrada = id }
        );
        return filas > 0;
        }

        public async Task<Entrada?> ObtenerEntrada(int id)
        {
            using var db = _ado.GetDbConnection();
            var sql = @"
                        SELECT *
                        FROM Entrada e
                        INNER JOIN Tarifa t ON e.idTarifa = t.idTarifa
                        INNER JOIN OrdenesCompra o ON e.idOrdenCompra = o.idOrdenCompra
                        INNER JOIN Usuario u ON o.idUsuario = u.idUsuario
                        INNER JOIN Cliente c ON u.DNI = c.DNI
                        WHERE e.idEntrada = @Id";

            var result = await db.QueryAsync<Entrada, Tarifa, OrdenesCompra, Usuario, Cliente, Entrada>(
                sql,
                (entrada, tarifa, orden, usuario, cliente) =>
                {
                    usuario.cliente = cliente;
                    orden.usuario = usuario;
                    entrada.tarifa = tarifa;
                    entrada.ordenesCompra = orden;
                    return entrada;
                },
                new { Id = id },
                splitOn: "idTarifa,idOrdenCompra,idUsuario,DNI"
            );

            return result.FirstOrDefault();
        }
        public async Task<IEnumerable<Entrada>> ObtenerTodos()
        {
            using var db = _ado.GetDbConnection();
             var sql = @"
                        SELECT *
                        FROM Entrada e
                        INNER JOIN Tarifa t ON e.idTarifa = t.idTarifa
                        INNER JOIN OrdenesCompra o ON e.idOrdenCompra = o.idOrdenCompra
                        INNER JOIN Usuario u ON o.idUsuario = u.idUsuario
                        INNER JOIN Cliente c ON u.DNI = c.DNI
                        ORDER BY e.idEntrada ASC";

            var result = await db.QueryAsync<Entrada, Tarifa, OrdenesCompra, Usuario, Cliente, Entrada>(
                sql,
                (entrada, tarifa, orden, usuario, cliente) =>
                {
                    usuario.cliente = cliente;
                    orden.usuario = usuario;
                    entrada.tarifa = tarifa;
                    entrada.ordenesCompra = orden;
                    return entrada;
                },
                splitOn: "idTarifa,idOrdenCompra,idUsuario,DNI"
            );

            return result;
        }
    }
}