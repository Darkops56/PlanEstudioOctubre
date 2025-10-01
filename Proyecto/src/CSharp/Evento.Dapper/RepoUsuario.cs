using Dapper;
using Evento.Core.Entidades;
using Evento.Core.Services.Repo;

namespace Evento.Dapper;

public class RepoUsuario : IRepoUsuario
{
    private readonly IAdo _ado;
    public RepoUsuario(IAdo ado) => _ado = ado;
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
        var query = "SELECT COUNT(1) FROM Usuario WHERE Email = @email";
        var count = await db.ExecuteScalarAsync<int>(query, new { email = nuevoEmail });

        return count == 1;
    }
    public async Task<IEnumerable<OrdenesCompra>> ObtenerComprasPorUsuario(int id)
    {
        using var db = _ado.GetDbConnection();
        return await db.QueryAsync<OrdenesCompra>("SELECT Fecha, Total, metodoPago, Estado FROM OrdenesCompra WHERE idUsuario = @Id", new { Id = id });
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
            role = usuario.Role.ToString()
        });
        return rows != 0 ? rows : 0;
    }
    public async Task<Usuario?> Login(string nuevoEmail, string nuevaContrasena)
    {
        var db = _ado.GetDbConnection();
        var sql = @"SELECT  u.*, c.*
                    FROM    Usuario u
                    JOIN    Cliente c ON u.DNI = c.DNI
                    WHERE   u.Email = @email
                    AND     Contrasena = @contrasena";

        var user = (await db.QueryAsync<Usuario, Cliente, Usuario>
                            (sql, (u, c) => { u.cliente = c; return u; },
                            new { email = nuevoEmail, contrasena = nuevaContrasena })
                    ).FirstOrDefault();

        return user;
    }
    public async Task<Usuario?> ObtenerPorEmail(string nuevoEmail)
    {
        var db = _ado.GetDbConnection();
        var query = "SELECT * FROM Usuario WHERE Email = @email";

        return await db.QueryFirstAsync<Usuario>(query, new { email = nuevoEmail });
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
            role = usuario.Role.ToString()
        });
        return rows > 0;
    }
}