using Evento.Core.Services;
using Evento.Core.Entidades;
using Evento.Core.DTOs;
using Evento.Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");
var key = builder.Configuration["Jwt:Key"];
var issuer = builder.Configuration["Jwt:Issuer"];

builder.Services.AddScoped<IAdo>(sp => new Ado(connectionString!));
builder.Services.AddScoped<IRepoEvento, RepoEvento>();
builder.Services.AddScoped<IRepoCliente, RepoCliente>();
builder.Services.AddScoped<IRepoEntrada, RepoEntrada>();
builder.Services.AddScoped<IRepoLocal, RepoLocal>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
    });

builder.Services.AddAuthorization();


var app = builder.Build();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Eventos API V1");
    c.RoutePrefix = "swagger";
});

app.Urls.Add("http://localhost:5090");

#region Endpoints

////////////// LOCALES //////////////
app.MapPost("/api/locales", async ([FromServices] IRepoLocal repo, [FromBody] LocalDto localDto) =>
{
    if(localDto == null)
        return Results.BadRequest("DTO vacio");

    var local = new Local
    {
        Nombre = localDto.Nombre,
        Ubicacion = localDto.Ubicacion
    };

    var resultado = await repo.InsertLocal(local);
    return Results.Ok(resultado);
});
app.MapGet("/api/locales", async ([FromServices] IRepoLocal repo) =>
{
    var locales = await repo.ObtenerTodos();
    return Results.Ok(locales);
});
app.MapGet("/api/locales/{id}", async ([FromServices] IRepoLocal repo, int id) =>
{
    var local = await repo.ObtenerPorId(id);
    return local != null ? Results.Ok(local) : Results.NotFound();
});
app.MapPut("/api/locales/{id}", async ([FromServices] IRepoLocal repo, [FromBody] LocalDto dto, int id) =>
{
    var localExistente = await repo.ObtenerPorId(id);
    if (localExistente == null) return Results.NotFound();

    localExistente.Nombre = dto.Nombre;
    localExistente.Ubicacion = dto.Ubicacion;

    var Updated = await repo.UpdateLocal(localExistente);
    return Results.Ok(Updated);
});
app.MapDelete("/api/locales/{id}", async ([FromServices] IRepoLocal repo, int id) =>
{
    var resultado = await repo.DeleteLocal(id);
    return Results.Ok(resultado);
});

////////////// SECTORES //////////////
app.MapGet("/api/locales/{id}/sectores", async ([FromServices] IRepoLocal repo, int idlocal) =>
{
    var sectores = await repo.ObtenerSectoresDelLocal(idlocal);
    return Results.Ok(sectores);
});
app.MapPost("/api/locales/{id}/sectores", async ([FromServices] IRepoLocal repo, [FromBody] SectorDto sectorDto, int idlocal) =>
{
    var local = await repo.ObtenerPorId(idlocal);
    if (local == null) return Results.NotFound();

    var sector = new Sector
    {
        
        local = local,
        Capacidad = sectorDto.Capacidad
    };

    var resultado = await repo.InsertSector(sector, idlocal);
    return Results.Ok(resultado);
});
app.MapPut("/api/sectores/{id}", async ([FromServices] IRepoLocal repo, [FromBody] SectorDto? dto, int id) =>
{
    var sectorExistente = await repo.ObtenerSectorPorId(id);
    if (sectorExistente == null) return Results.NotFound();

    sectorExistente.Capacidad = dto.Capacidad;
    var resultado = await repo.UpdateSector(sectorExistente, id);
    return Results.Ok(resultado);
});
app.MapDelete("/api/sectores/{id}", async ([FromServices] IRepoLocal repo, int id) =>
{
    var resultado = await repo.DeleteSector(id);
    return Results.Ok(resultado);
});

////////////// EVENTOS //////////////
app.MapGet("/api/eventos", async ([FromServices] IRepoEvento repo) =>
{
    var eventos = await repo.ObtenerTodos();
    return eventos.Any() ? Results.Ok(eventos) : Results.NotFound();
});
app.MapGet("/api/evento/{id}", async ([FromServices] IRepoEvento repo, int id) =>
{
    var evento = await repo.ObtenerEventoPorId(id);
    if (evento == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(evento);
});
app.MapPost("/api/eventos", async ([FromServices] IRepoEvento repo, [FromBody] EventoDto dto) =>
{
    var TipoEvento = await repo.ObtenerTipoEventoPorId(dto.idTipoEvento);
    var evento = new Eventos
    {
        Nombre = dto.Nombre,
        fechaFin = dto.fechaFin,
        fechaInicio = dto.fechaInicio,
        tipoEvento = TipoEvento
    };
    var nuevoEvento = await repo.InsertEvento(evento);
    if (nuevoEvento == null || nuevoEvento.ToString() == "")
    {
        return Results.BadRequest();
    }
    return Results.Created();
});

app.MapPut("/api/eventos/{id}", async ([FromServices] IRepoEvento repo, [FromBody] Eventos? evento, int id) =>
{
    var UpdateLocal = await repo.UpdateEvento(evento);

    return Results.Ok(UpdateLocal);
});
app.MapGet("/api/{id}/publicar", async ([FromServices] IRepoEvento repo, int id) =>
{
    var evento = await repo.ObtenerEventoPorId(id);
    throw new NotImplementedException();
});
app.MapGet("/api/{id}/cancelar", async ([FromServices] IRepoEvento repo, int id) =>
{
    var evento = await repo.ObtenerEventoPorId(id);
    throw new NotImplementedException();
});

////////////// FUNCIONES //////////////
app.MapPost("/api/funciones", async ([FromServices] IRepoFuncion repo, [FromBody] Funcion funcion) =>
{
    var resultado = await repo.InsertFuncion(funcion);

    return Results.Ok(resultado);
});
app.MapGet("/api/funciones", async ([FromServices] IRepoFuncion repo) =>
{
    var resultado = await repo.ObtenerTodos();

    return Results.Ok(resultado);
});
app.MapGet("/api/funciones/{id}", async ([FromServices] IRepoFuncion repo, int id) =>
{
    var resultado = await repo.ObtenerPorId(id);

    return Results.Ok(resultado);
});

app.MapPut("/api/funciones/{id}", async ([FromServices] IRepoFuncion repo, int id) =>
{
    var funcion = await repo.ObtenerPorId(id);
    var resultado = await repo.UpdateFuncion(funcion);

    return Results.Ok(resultado);
});
app.MapPost("/api/funciones/{id}/cancelar", ([FromServices] IRepoFuncion repo, int id) =>
{
    throw new NotImplementedException();
});

////////////// TARIFAS - STOCK //////////////
app.MapPost("/api/tarifas", async ([FromServices] IRepoTarifa repo, [FromBody] Tarifa tarifa) =>
{
    var tarifas = await repo.InsertTarifa(tarifa);

    return Results.Ok(tarifas);
});
app.MapGet("/api/funciones/{id}/tarifas", async ([FromServices] IRepoFuncion repo, int id) =>
{
    var tarifaFuncion = await repo.ObtenerTarifasDeFuncion(id);

    return Results.Ok(tarifaFuncion);
});
app.MapPut("/api/tarifas/{id}", async ([FromServices] IRepoTarifa repo, [FromBody] Tarifa tarifa) =>
{
    var resultado = await repo.UpdateTarifa(tarifa);

    return Results.Ok(resultado);
});
app.MapGet("/api/tarifas/{id}", ([FromServices] IRepoTarifa repo, int id) =>
{
    var tarifa = repo.ObtenerPorId(id);

    return Results.Ok(tarifa);
});

////////////// CLIENTE //////////////
app.MapGet("/api/clientes", async ([FromServices] IRepoCliente repo) =>
{
    var clientes = await repo.ObtenerTodos();
    return clientes.Any() ? Results.Ok(clientes) : Results.NotFound();
});
app.MapGet("/api/cliente/{id}", async (IRepoCliente repo, int id) =>
{
    var cliente = await repo.ObtenerPorId(id);
    if (cliente == null || cliente.ToString() == "")
    {
        return Results.NotFound();
    }
    return Results.Ok(cliente);
});
app.MapPost("/api/cliente", async ([FromServices] IRepoCliente repo, [FromBody] Cliente cliente) =>
{
    var nuevoCliente = await repo.InsertCliente(cliente);
    if (nuevoCliente == null || nuevoCliente.ToString() == "")
    {
        return Results.BadRequest();
    }
    return Results.Created();
});
app.MapPut("/api/cliente/{id}", async ([FromServices] IRepoCliente repo, int id) =>
{
    var cliente = await repo.ObtenerPorId(id);
    var UpdateCliente = await repo.UpdateCliente(cliente);
    return Results.Ok(UpdateCliente);
});

////////////// ORDENES DE COMPRA //////////////
/* app.MapPost("/api/ordenes", () =>
{
    throw new NotImplementedException();
});

app.MapGet("/api/ordenes", () =>
{
    throw new NotImplementedException();
});
app.MapGet("/api/ordenes/{id}", ([FromBody] int id) =>
{
    throw new NotImplementedException();
});
app.MapPost("/api/ordenes/{id}/pagar", () =>
{
    throw new NotImplementedException();
});
app.MapPost("/api/ordenes/{id}/cancelar", () =>
{
    throw new NotImplementedException();
}); */

////////////// ENTRADAS //////////////
app.MapGet("/api/entradas", ([FromServices] IRepoEntrada repo) =>
{
    var entradas = repo.ObtenerTodos();

    return Results.Ok(entradas);
});
app.MapGet("/api/entradas/{id}", ([FromServices] IRepoEntrada repo, int id) =>
{
    var entrada = repo.ObtenerEntrada(id);

    return Results.Ok(entrada);
});
app.MapPost("/api/entradas/{id}/anular", ([FromServices] IRepoEntrada repo, int id) =>
{
    var entrada = repo.ObtenerEntrada(id);
    throw new NotImplementedException();
});

////////////// LOGIN //////////////
app.MapPost("/api/auth/register", async ([FromServices] IRepoUsuario repoUsuario, [FromServices] IRepoCliente repoCliente, [FromBody] Usuario nuevoUsuario) =>
{
    if (await repoUsuario.ExisteUsuarioPorEmail(nuevoUsuario.Email))
        return Results.BadRequest("Ya Existe El Usuario");

    if (!await repoCliente.ExistePorDNI(nuevoUsuario.cliente.DNI))
        await repoCliente.InsertCliente(nuevoUsuario.cliente);

    nuevoUsuario.Role = "Usuario";
    await repoUsuario.InsertUsuario(nuevoUsuario);
    
    return Results.Ok(new
    {
        mensaje = "Usuario Creado Ccon Exito",
        usuario = new
        {
            nuevoUsuario.idUsuario,
            nuevoUsuario.Apodo,
            nuevoUsuario.Email,
            nuevoUsuario.Role,
            Cliente = nuevoUsuario.cliente

        }
    });
});

app.MapPost("/api/auth/login", async ([FromServices] IRepoUsuario repo, [FromBody] LoginDto login) =>
{
    var usuario = await repo.Login(login.Email, login.Contrasena);

    if (usuario is null)
        return Results.Unauthorized();

    var claims = new[]
    {
        new Claim(ClaimTypes.Name, usuario.Email),
        new Claim(ClaimTypes.Role, usuario.Role),
        new Claim(ClaimTypes.Role, string.IsNullOrEmpty(usuario.Role) ? "Usuario" : usuario.Role),
        new Claim("Apodo", usuario.Apodo),
        new Claim("DNI", usuario.cliente.DNI.ToString()),
        new Claim("NombreCompleto", usuario.cliente.nombreCompleto),
        new Claim("Telefono", usuario.cliente.Telefono ?? "")
    };

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("olandesa51"));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        issuer: "miapp",
        audience: null,
        claims: claims,
        expires: DateTime.UtcNow.AddHours(2),
        signingCredentials: creds);

    return Results.Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
});
app.MapGet("/api/auth/me", (HttpContext ctx) =>
{
    var email = ctx.User.Identity?.Name;
    var rol = ctx.User.FindFirst(ClaimTypes.Role)?.Value;
    var apodo = ctx.User.FindFirst("Apodo")?.Value;
    var dni = ctx.User.FindFirst("DNI")?.Value;
    var nombre = ctx.User.FindFirst("nombreCompleto")?.Value;
    return new
    {
        Email = email,
        Rol = rol,
        Apodo = apodo,
        DNI = dni,
        Nombre = nombre
    };
})
.RequireAuthorization();

#endregion

app.Run();