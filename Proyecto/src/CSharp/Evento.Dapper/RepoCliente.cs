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
            using var db = _ado.GetConnection();
            var rows = await db.ExecuteAsync("DELETE FROM Cliente WHERE DNI = @Id", new { Id = id });
            return rows > 0;
        }

        public async Task<int> InsertCliente(Cliente cliente)
        {
            using var db = _ado.GetConnection();
            var rows = await db.ExecuteAsync("INSERT INTO Cliente(DNI, NombreCompleto, Email, Telefono, Contrasena) VALUES(@dni, @nombrecompleto, @email, @telefono, @contrasena)", new
            {
                dni = cliente.DNI,
                nombrecompleto = cliente.nombreCompleto,
                email = cliente.Email,
                telefono = cliente.Telefono,
                contrasena = cliente.Contrasena
            });
            return rows > 0 ? rows : 0;
        }

        public Task<IEnumerable<RegistroCompra>> ObtenerComprasPorCliente(int id)
        {
            using var db = _ado.GetConnection();
            return db.QueryAsync<RegistroCompra>("SELECT * FROM RegistroCompra WHERE DNI = @Id", new { Id = id });
        }

        public Task<IEnumerable<Entrada>> ObtenerEntradasPorCliente(int id)
        {
            using var db = _ado.GetConnection();
            return db.QueryAsync<Entrada>("SELECT * FROM Entrada WHERE DNI = @Id", new{ Id = id });
        }

        public async Task<Cliente?> ObtenerPorId(int id)
        {
            using var db = _ado.GetConnection();
            return await db.QueryFirstOrDefaultAsync<Cliente>("SELECT * FROM Cliente WHERE DNI = @Id", new{ Id = id });
        }

        public async Task<IEnumerable<Cliente>> ObtenerTodos()
        {
            using var db = _ado.GetConnection();
            return await db.QueryAsync<Cliente>("SELECT * FROM Cliente");
        }

        public async Task<bool> UpdateCiente(Cliente cliente)
        {
            using var db = _ado.GetConnection();
            string query = "UPDATE Cliente SET DNI = @dni, nombreCompleto = @NombreCompleto, Email = @email, Telefono = @telefono, Contrasena = @contrasena";
            var rows = await db.ExecuteAsync(query, new
            {
                dni = cliente.DNI,
                nombrecompleto = cliente.nombreCompleto,
                email = cliente.Email,
                telefono = cliente.Telefono,
                contrasena = cliente.Contrasena
            });
            return rows > 0;
        }
    }
}