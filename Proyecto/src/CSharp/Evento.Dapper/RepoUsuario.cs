using Dapper;
using Evento.Core.Entidades;
using Evento.Core.Services.Repo;
using Evento.Core.Services.Utility;

namespace Evento.Dapper;

public class RepoUsuario : IRepoUsuario
{
    private readonly IAdo _ado;
    public RepoUsuario(IAdo ado) => _ado = ado;

    public async Task<int> InsertUsuario(Usuario usuario)
    {
        var db = _ado.GetDbConnection();
        var query = @"INSERT INTO Usuario (Apodo, Email, Contrasena, DNI, Roles)
                      VALUES (@apodo, @email, @contrasena, @dni, @role)";
        return await db.ExecuteAsync(query, new
        {
            apodo = usuario.Apodo,
            email = usuario.Email,
            contrasena = usuario.Contrasena,
            dni = usuario.cliente.DNI,
            role = UniqueFormatStrings.NormalizarString(usuario.Role.ToString())
        });
    }

    public async Task<Usuario?> ObtenerPorEmail(string nuevoEmail)
    {
        var db = _ado.GetDbConnection();
        var sql = @"SELECT u.idUsuario, u.Apodo, u.Email, u.Contrasena, u.Roles, u.DNI as UsuarioDNI,
                   c.DNI as ClienteDNI, c.nombreCompleto, c.Telefono
            FROM Usuario u
            JOIN Cliente c ON u.DNI = c.DNI
            WHERE u.Email = @email";

        var user = (await db.QueryAsync<Usuario, Cliente, Usuario>(
            sql,
            (u, c) => { u.cliente = c; return u; },
            new { email = nuevoEmail },
            splitOn: "ClienteDNI" 
        )).FirstOrDefault();

        return user;
    }
    public async Task<Usuario?> ObtenerPorId(int id)
    {
        var db = _ado.GetDbConnection();
        var query = "SELECT * FROM Usuario WHERE idUsuario = @idusuario";
        return await db.QueryFirstOrDefaultAsync<Usuario>(query, new { idusuario = id });
    }

    public async Task<bool> UpdateUsuario(Usuario usuario)
    {
        var db = _ado.GetDbConnection();
        var query = @"UPDATE Usuario
                      SET Apodo = @apodo,
                          Email = @email,
                          Contrasena = @contrasena,
                          DNI = @dni,
                          Roles = @role
                      WHERE idUsuario = @idusuario";
        var rows = await db.ExecuteAsync(query, new
        {
            apodo = usuario.Apodo,
            email = usuario.Email,
            contrasena = usuario.Contrasena,
            dni = usuario.cliente.DNI,
            role = UniqueFormatStrings.NormalizarString(usuario.Role.ToString()),
            idusuario = usuario.idUsuario
        });
        return rows > 0;
    }

    public async Task<bool> DeleteUsuario(int id)
    {
        var db = _ado.GetDbConnection();
        var query = "DELETE FROM Usuario WHERE idUsuario = @idusuario";
        var rows = await db.ExecuteAsync(query, new { idusuario = id });
        return rows > 0;
    }

    public async Task<bool> ExisteUsuarioPorEmail(string nuevoEmail)
    {
        var db = _ado.GetDbConnection();
        var query = "SELECT COUNT(1) FROM Usuario WHERE Email = @email";
        var count = await db.ExecuteScalarAsync<int>(query, new { email = nuevoEmail });
        return count > 0;
    }

    public async Task<IEnumerable<Usuario>> ObtenerTodos()
    {
        var db = _ado.GetDbConnection();
        var query = "SELECT * FROM Usuario";
        return await db.QueryAsync<Usuario>(query);
    }

    public async Task<IEnumerable<OrdenesCompra>> ObtenerComprasPorUsuario(int id)
    {
        var db = _ado.GetDbConnection();
        var query = "SELECT Fecha, Total, metodoPago, Estado FROM OrdenesCompra WHERE idUsuario = @Id";
        return await db.QueryAsync<OrdenesCompra>(query, new { Id = id });
    }
}
