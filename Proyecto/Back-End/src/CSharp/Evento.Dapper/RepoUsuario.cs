using Dapper;
using Evento.Core.DTOs;
using Evento.Core.Entidades;
using Evento.Core.Services.Enums;
using Evento.Core.Services.Repo;
using Evento.Core.Services.Utility;

namespace Evento.Dapper;

public class RepoUsuario : IRepoUsuario
{
    private readonly IAdo _ado;
    public RepoUsuario(IAdo ado) => _ado = ado;

    public async Task<int> InsertUsuario(Usuario usuario)
    {
        using var db = _ado.GetDbConnection();
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
        using var db = _ado.GetDbConnection();
        var sql = @"
                    SELECT u.idUsuario, u.Apodo, u.Email, u.Contrasena, u.Roles, u.DNI,
                        c.DNI, c.nombreCompleto, c.Telefono
                    FROM Usuario u
                    JOIN Cliente c ON u.DNI = c.DNI
                    WHERE u.Email = @email";

        var user = (await db.QueryAsync<Usuario, Cliente, Usuario>(
            sql,
            (u, c) => { u.cliente = c; return u; },
            new { email = nuevoEmail },
            splitOn: "DNI" 
        )).FirstOrDefault();

        return user;
    }
    public async Task<Usuario?> ObtenerPorId(int id)
    {
        using var db = _ado.GetDbConnection();
        var sql = @"SELECT u.idUsuario, u.Apodo, u.Email, u.Contrasena, u.Roles, u.DNI,
                           c.DNI, c.nombreCompleto, c.Telefono
                    FROM Usuario u
                    JOIN Cliente c ON u.DNI = c.DNI
                    WHERE u.idUsuario = @idusuario
                    LIMIT 1";

        var user = (await db.QueryAsync<Usuario, Cliente, Usuario>(
            sql,
            (u, c) => { u.cliente = c; return u; },
            new { idusuario = id },
            splitOn: "DNI"
        )).FirstOrDefault();

        return user;
    }
    public async Task<bool> UpdateUsuario(Usuario usuario)
    {
        using var db = _ado.GetDbConnection();
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
        using var db = _ado.GetDbConnection();
        var query = "DELETE FROM Usuario WHERE idUsuario = @idusuario";
        var rows = await db.ExecuteAsync(query, new { idusuario = id });
        return rows > 0;
    }
    public async Task<bool> ExisteUsuarioPorEmail(string nuevoEmail)
    {
        using var db = _ado.GetDbConnection();
        var query = "SELECT COUNT(1) FROM Usuario WHERE Email = @email";
        var count = await db.ExecuteScalarAsync<int>(query, new { email = nuevoEmail });
        return count > 0;
    }
    public async Task<IEnumerable<Usuario>> ObtenerTodos()
    {
        using var db = _ado.GetDbConnection();
        var sql = @"SELECT u.idUsuario, u.Apodo, u.Email, u.Contrasena, u.Roles, u.DNI,
                           c.DNI, c.nombreCompleto, c.Telefono
                    FROM Usuario u
                    JOIN Cliente c ON u.DNI = c.DNI";

        var users = (await db.QueryAsync<Usuario, Cliente, Usuario>(
            sql,
            (u, c) => { u.cliente = c; return u; },
            splitOn: "DNI"
        ));

        return users;
    }
    public async Task<IEnumerable<OrdenesCompra>> ObtenerComprasPorUsuario(int id)
    {
        using var db = _ado.GetDbConnection();
        var sqlOrdenes = @"
        SELECT o.idOrdenCompra, o.Fecha, o.Total, o.metodoPago, o.Estado
        FROM OrdenesCompra o
        WHERE o.idUsuario = @Id";

        var ordenes = (await db.QueryAsync<OrdenesCompra>(sqlOrdenes, new { Id = id })).ToList();

        if (!ordenes.Any())
            return ordenes;

        var idsOrdenes = ordenes.Select(o => o.idOrdenCompra).ToArray();

        var sqlEntradas = @"
            SELECT e.idEntrada, e.Estado, e.PrecioPagado, e.idOrdenCompra,
                t.idTarifa, t.Tipo, t.Precio, t.Stock, t.Estado AS EstadoTarifa
            FROM Entrada e
            INNER JOIN Tarifa t ON e.idTarifa = t.idTarifa
            WHERE e.idOrdenCompra IN @IdsOrdenes";

        var entradas = await db.QueryAsync<Entrada, Tarifa, Entrada>(
            sqlEntradas,
            (entrada, tarifa) =>
            {
                entrada.tarifa = tarifa;
                return entrada;
            },
            new { IdsOrdenes = idsOrdenes },
            splitOn: "idTarifa"
        );

        foreach (var orden in ordenes)
        {
            orden.entradas = entradas.Where(e => e.ordenesCompra.idOrdenCompra == orden.idOrdenCompra).ToList();
        }

        return ordenes;
    }
}
