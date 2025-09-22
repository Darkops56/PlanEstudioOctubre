using Dapper;
using Evento.Core.Entidades;
using Evento.Core.Services;

namespace Evento.Dapper
{
    public class RepoUsuario : IRepoUsuario
    {
        private readonly Ado _ado;
        public async Task<bool> DeleteUsuario(int id)
        {
            var db = _ado.GetDbConnection();
            var query = "DELETE FROM Usuario WHERE idUsuario = @idusuario";
            var rows = await db.ExecuteAsync(query, new { idevento = id });

            return rows > 0;
        }

        public async Task<bool> ExisteUsuarioPorEmail(string nuevoEmail)
        {
            var db = _ado.GetDbConnection();
            var query = "SELECT * FROM Usuario WHERE Email = @email";
            var rows = await db.ExecuteAsync(query, new { email = nuevoEmail });
            if (rows > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<int> InsertUsuario(Usuario usuario)
        {
            var db = _ado.GetDbConnection();
            var query = "INSERT INTO Usuario(Apodo, Email, Contrasena, DNI, Roles) VALUES(@apodo, @email, @contrasena, @dni, @role)";
            var rows = await db.ExecuteAsync(query, new
            {
                apodo = usuario.Apodo,
                email = usuario.Email,
                contrasena = usuario.Contrasena,
                dni = usuario.cliente.DNI,
                role = usuario.Role
            });
            return rows > 0 ? rows : 0;
        }

        public async Task<Usuario?> Login(string nuevoEmail, string nuevaContrasena)
        {
            var db = _ado.GetDbConnection();
            var query = "SELECT * FROM Usuario JOIN Cliente USING (DNI) WHERE Email = @email AND Contrasena = @contrasena";

            return await db.QueryFirstAsync<Usuario>(query, new{ email = nuevoEmail, contrasena = nuevaContrasena });
        }

        public async Task<Usuario?> ObtenerPorEmail(string nuevoEmail)
        {
            var db = _ado.GetDbConnection();
            var query = "SELECT * FROM Usuario WHERE Email = @email";

            return await db.QueryFirstAsync<Usuario>(query, new { email = nuevoEmail});
        }

        public async Task<Usuario?> ObtenerPorId(int id)
        {
            var db = _ado.GetDbConnection();
            var query = "SELECT * FROM Usuario WHERE idUsuario = @idusuario";

            return await db.QueryFirstAsync<Usuario?>(query, new { idusuario = id });
        }

        public async Task<IEnumerable<Usuario>> ObtenerTodos()
        {
            var db = _ado.GetDbConnection();
            var query = "SELECT * FROM Usuario";

            return await db.QueryAsync<Usuario>(query);
        }

        public async Task<bool> UpdateUsuario(Usuario usuario)
        {
            var db = _ado.GetDbConnection();
            var query = "UPDATE Usuario SET Apodo = @apodo, Email = @email, Contrasena = @contrasena, DNI = @dni, Roles = @role WHERE idUsuario = @idusuario";
            var rows = await db.ExecuteAsync(query, new
            {
                apodo = usuario.Apodo,
                email = usuario.Email,
                contrasena = usuario.Contrasena,
                idusuario = usuario.idUsuario,
                dni = usuario.cliente.DNI,
                role = usuario.Role
            });
            return rows > 0;
        }
    }
}