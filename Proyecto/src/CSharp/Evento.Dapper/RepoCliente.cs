using MySql.Data.MySqlClient;
using Dapper;
using Evento.Core.Entidades;
using Evento.Core.Services;

namespace Evento.Dapper
{
    public class RepoCliente : IRepoCliente
    {
        private readonly IAdo _ado;

        public RepoCliente(IAdo ado) => _ado = ado;

        public async Task<bool> DeleteCliente(int id)
        {
            using var db = _ado.GetDbConnection();
            var rows = await db.ExecuteAsync("DELETE FROM Cliente WHERE DNI = @Id", new { Id = id });
            return rows > 0;
        }

        public Task<bool> ExistePorDNI(int dni)
        {
            throw new NotImplementedException();
        }

        public async Task<int> InsertCliente(Cliente cliente)
        {
            using var db = _ado.GetDbConnection();
            var rows = await db.ExecuteAsync("INSERT INTO Cliente(DNI, NombreCompleto, Telefono) VALUES(@dni, @nombrecompleto, @telefono)", new
            {
                dni = cliente.DNI,
                nombrecompleto = cliente.nombreCompleto,
                telefono = cliente.Telefono
            });
            return rows > 0 ? rows : 0;
        }

        public async Task<IEnumerable<RegistroCompra>> ObtenerComprasPorCliente(int id)
        {
            using var db = _ado.GetDbConnection();
            return await db.QueryAsync<RegistroCompra>("SELECT * FROM RegistroCompra WHERE DNI = @Id", new { Id = id });
        }

        public async Task<IEnumerable<Entrada>> ObtenerEntradasPorCliente(int id)
        {
            using var db = _ado.GetDbConnection();
            return await db.QueryAsync<Entrada>("SELECT * FROM Entrada WHERE DNI = @Id", new{ Id = id });
        }

        public async Task<Cliente?> ObtenerPorId(int id)
        {
            using var db = _ado.GetDbConnection();
            return await db.QueryFirstOrDefaultAsync<Cliente>("SELECT * FROM Cliente WHERE DNI = @Id", new{ Id = id });
        }

        public async Task<IEnumerable<Cliente>> ObtenerTodos()
        {
            using var db = _ado.GetDbConnection();
            return await db.QueryAsync<Cliente>("SELECT * FROM Cliente");
        }

        public async Task<bool> UpdateCliente(Cliente cliente)
        {
            using var db = _ado.GetDbConnection();
            string query = "UPDATE Cliente SET DNI = @dni, nombreCompleto = @NombreCompleto, Telefono = @telefono WHERE DNI = @dni";
            var rows = await db.ExecuteAsync(query, new
            {
                dni = cliente.DNI,
                nombrecompleto = cliente.nombreCompleto,
                telefono = cliente.Telefono,
            });
            return rows > 0;
        }
    }
}