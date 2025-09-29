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

         public async Task<int> InsertOrdenCompra(OrdenesCompra orden)
        {
            if (orden.usuario == null) throw new Exception("El usuario es obligatorio");

            using var db = _ado.GetDbConnection();
            var sql = @"INSERT INTO OrdenesCompra(idUsuario, Fecha, Total, MetodoPago, Estado)
                        VALUES(@IdUsuario, @Fecha, @Total, @MetodoPago, @Estado)";
            var rows = await db.ExecuteAsync(sql, new
            {
                IdUsuario = orden.usuario.idUsuario,
                orden.Fecha,
                orden.Total,
                orden.metodoPago,
                orden.estado
            });
            return rows > 0 ? rows : 0;
        }

        public async Task<bool> UpdateOrdenCompra(OrdenesCompra orden)
        {
            if (orden.usuario == null) throw new Exception("El usuario es obligatorio");

            using var db = _ado.GetDbConnection();
            var sql = @"UPDATE OrdenesCompra 
                        SET idUsuario = @IdUsuario, Fecha = @Fecha, Total = @Total, MetodoPago = @MetodoPago, Estado = @Estado
                        WHERE idOrdenCompra = @IdOrdenCompra";
            var rows = await db.ExecuteAsync(sql, new
            {
                IdUsuario = orden.usuario.idUsuario,
                orden.Fecha,
                orden.Total,
                orden.metodoPago,
                orden.estado,
                orden.idOrdenCompra
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
            return await db.QueryFirstOrDefaultAsync<OrdenesCompra>(
                "SELECT * FROM OrdenesCompra WHERE idOrdenCompra = @Id", new { Id = id });
        }

        public async Task<IEnumerable<OrdenesCompra>> ObtenerOrdenesCompra()
        {
            using var db = _ado.GetDbConnection();
            return await db.QueryAsync<OrdenesCompra>("SELECT * FROM OrdenesCompra");
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

            var orden = await ObtenerOrdenCompra(idOrdenCompra);
            if (orden == null) throw new Exception("La orden no existe");

            if (orden.estado == "Pagada")
                throw new Exception("La orden ya está pagada");

            var rows = await db.ExecuteAsync(
                "UPDATE OrdenesCompra SET Estado = 'Pagada' WHERE idOrdenCompra = @IdOrdenCompra",
                new { IdOrdenCompra = idOrdenCompra });

            return string.Empty;
        }

        public async Task<string> CancelarOrdenCompra(int idOrdenCompra)
        {
            using var db = _ado.GetDbConnection();

            var orden = await ObtenerOrdenCompra(idOrdenCompra);
            if (orden == null) throw new Exception("La orden no existe");

            if (orden.estado == "Cancelada")
                throw new Exception("La orden ya está cancelada");

            var rows = await db.ExecuteAsync(
                "UPDATE OrdenesCompra SET Estado = 'Cancelada' WHERE idOrdenCompra = @IdOrdenCompra",
                new { IdOrdenCompra = idOrdenCompra });

            return string.Empty;
        }
    }
}