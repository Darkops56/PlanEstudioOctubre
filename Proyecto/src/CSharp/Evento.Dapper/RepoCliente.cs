using MySql.Data.MySqlClient;
using Dapper;
using Evento.Core.Entidades;
using Evento.Core.Services.Repo;

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

        public async Task<bool> ExistePorDNI(int dni)
        {
            using var db = _ado.GetDbConnection();
            var query = await db.QueryFirstOrDefaultAsync("SELECT * FROM Cliente WHERE DNI = @Dni LIMIT 1", new { Dni = dni });
            if (query == null)
            {
                return false;
            }
            return true;
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
        public async Task<IEnumerable<Entrada>> ObtenerEntradasPorCliente(int id)
        {
            using var db = _ado.GetDbConnection();
            return await db.QueryAsync<Entrada>("SELECT * FROM Entrada JOIN OrdenesCompra oc USING (idOrdenCompra) WHERE oc.DNI = @Id", new{ Id = id });
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
            string query = "UPDATE Cliente SET nombreCompleto = @NombreCompleto, Telefono = @telefono WHERE DNI = @dni";
            var rows = await db.ExecuteAsync(query, new
            {
                nombrecompleto = cliente.nombreCompleto,
                telefono = cliente.Telefono,
            });
            return rows > 0;
        }
    }
}