using Evento.Core.Entidades;
using MySql.Data.MySqlClient;
using Dapper;
using System;
using Evento.Core.Services;
using Evento.Dapper;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI.Common;
using Mysqlx.Crud;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");

builder.Services.AddScoped<IAdo>(sp => new Ado(connectionString!));
builder.Services.AddScoped<IRepoEvento, RepoEvento>();
builder.Services.AddScoped<IRepoCliente, RepoCliente>();
builder.Services.AddScoped<IRepoEntrada, RepoEntrada>();

var app = builder.Build();

app.UseRouting();
app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Eventos API V1");
    c.RoutePrefix = "swagger";
});

app.Urls.Add("http://localhost:5090");

#region Endpoints
////////////// EVENTOS //////////////
app.MapGet("/api/eventos", async (IRepoEvento repo) =>
{
    var eventos = await repo.ObtenerTodos();
    return eventos.Any() ? Results.Ok(eventos) : Results.NotFound();
});
app.MapGet("/api/evento/{int:id}", async ([FromBody] int id, IRepoEvento repo) =>
{
    var evento = await repo.ObtenerEvento(id);
    if (evento == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(evento);
});
app.MapPost("/api/eventos", async ([FromBody] Eventos evento, IRepoEvento repo) =>
{
    var nuevoEvento = await repo.InsertEvento(evento);
    if (nuevoEvento == null || nuevoEvento.ToString() == "")
    {
        return Results.BadRequest();
    }
    return Results.Created();
});

app.MapPut("/api/eventos/{int:id}", async (IRepoEvento repo, [FromBody] int id, Eventos? evento) =>
{
    var UpdateLocal = await repo.UpdateEvento(evento);

    return Results.Ok(UpdateLocal);
});
app.MapGet("/api/{int:id}/publicar", async ([FromBody] int id, IRepoEvento repo) =>
{
    var evento = await repo.ObtenerEvento(id);
});
app.MapGet("/api/{int:id}/cancelar", async ([FromBody] int id, IRepoEvento repo) =>
{
    var evento = await repo.ObtenerEvento(id);
});
////////////// FUNCIONES //////////////
app.MapPost("/api/funciones", async (IRepoFuncion repo, [FromBody] Funcion funcion) =>
{
    var resultado = await repo.InsertFuncion(funcion);

    return Results.Ok(resultado);
});
app.MapGet("/api/funciones", async (IRepoFuncion repo) =>
{
    var resultado = await repo.ObtenerTodos();

    return Results.Ok(resultado);
});
app.MapGet("/api/funciones/{int:id}", async (IRepoFuncion repo, [FromBody] int id) =>
{
    var resultado = await repo.ObtenerPorId(id);

    return Results.Ok(resultado);
});

app.MapPut("/api/funciones/{int:id}", async (IRepoFuncion repo, [FromBody] int id) =>
{
    var funcion = await repo.ObtenerPorId(id);
    var resultado = await repo.UpdateFuncion(funcion);

    return Results.Ok(resultado);
});
app.MapPost("/api/funciones/{int:id}/cancelar", async (IRepoFuncion repo, [FromBody] int id) =>
{
    throw new NotImplementedException();
});

////////////// CLIENTE //////////////
app.MapGet("/api/clientes", async (IRepoCliente repo) =>
{
    var clientes = await repo.ObtenerTodos();
    return clientes.Any() ? Results.Ok(clientes) : Results.NotFound();
});
app.MapGet("/api/cliente/{int:id}", async ([FromBody] int id, IRepoCliente repo) =>
{
    var cliente = await repo.ObtenerPorId(id);
    if (cliente == null || cliente.ToString() == "")
    {
        return Results.NotFound();
    }
    return Results.Ok(cliente);
});
app.MapPost("/api/cliente", async ([FromBody] Cliente cliente, IRepoCliente repo) =>
{
    var nuevoCliente = await repo.InsertCliente(cliente);
    if (nuevoCliente == null || nuevoCliente.ToString() == "")
    {
        return Results.BadRequest();
    }
    return Results.Created();
});
app.MapPut("/api/cliente/{int:id}", async ([FromBody] int id, IRepoCliente repo) =>
{
    var cliente = await repo.ObtenerPorId(id);
    var UpdateCliente = await repo.UpdateCliente(cliente);
    return Results.Ok(UpdateCliente);
});

////////////// TARIFAS - STOCK //////////////
app.MapPost("/api/tarifas", async (IRepoTarifa repo, [FromBody] Tarifa tarifa) =>
{
    var tarifas = await repo.InsertTarifa(tarifa);

    return Results.Ok(tarifas);
});
app.MapGet("/api/funciones/{int:id}/tarifas", async (IRepoFuncion repo, [FromBody] int id) =>
{
    var tarifaFuncion = await repo.ObtenerTarifasDeFuncion(id);

    return Results.Ok(tarifaFuncion);
});
app.MapPut("/api/tarifas/{int:id}", async (IRepoTarifa repo, [FromBody] Tarifa tarifa) =>
{
    var resultado = repo.UpdateTarifa(tarifa);

    return Results.Ok(resultado);
});
app.MapGet("/api/tarifas/{int:id}", (IRepoTarifa repo, [FromBody] int id) =>
{
    var tarifa = repo.ObtenerPorId(id);

    return Results.Ok(tarifa);
});
////////////// ENTRADAS //////////////
app.MapGet("/api/entradas", (IRepoEntrada repo) =>
{
    var entradas = repo.ObtenerTodos();

    return Results.Ok(entradas);
});
app.MapGet("/api/entradas/{int:id}", (IRepoEntrada repo, [FromBody] int id) =>
{
    var entrada = repo.ObtenerEntrada(id);

    return Results.Ok(entrada);
});
app.MapPost("/api/entradas/{int:id}/anular", (IRepoEntrada repo, [FromBody] int id) =>
{
    var entrada = repo.ObtenerEntrada(id);
});
////////////// LOCALES //////////////
app.MapPost("/api/locales", async (IRepoLocal repo, [FromBody] Local local) =>
{
    var resultado = await repo.InsertLocal(local);
    return Results.Ok(resultado);
});
app.MapGet("/api/locales", async (IRepoLocal repo) =>
{
    var locales = await repo.ObtenerTodos();

    return Results.Ok(locales);
});
app.MapGet("/api/locales/{int:id}", async (IRepoLocal repo, [FromBody] int id) =>
{
    var local = await repo.ObtenerPorId(id);

    return Results.Ok(local);
});
app.MapPut("/api/locales/{int:id}", async (IRepoLocal repo, [FromBody] int id, Local local) =>
{
    var UpdateLocal = await repo.UpdateLocal(local);

    return Results.Ok(UpdateLocal);
});
app.MapDelete("/api/locales/{int:id}", async (IRepoLocal repo, [FromBody] int id) =>
{
    var resultado = await repo.DeleteLocal(id);

    return Results.Ok(resultado);
});

app.MapGet("/api/locales/{int:id}/sectores", async (IRepoLocal repo, [FromBody] int idlocal) =>
{
    var sectores = await repo.ObtenerSectoresDelLocal(idlocal);

    return Results.Ok(sectores);
});

////////////// SECTORES //////////////
app.MapPost("/api/locales/{int:id}/sectores", async (IRepoLocal repo, [FromBody] int idlocal, Sector sector) =>
{
    var local = await repo.ObtenerPorId(idlocal);
    var resultado = await repo.InsertSector(sector, idlocal);

    return Results.Ok(resultado);
});
app.MapPut("/api/sectores/{int:id}", async (IRepoLocal repo, [FromBody] int id, Sector? sector) =>
{
    var resultado = await repo.UpdateSector(sector);

    return Results.Ok(resultado);
});
app.MapDelete("/api/sectores/{int:id}", async (IRepoLocal repo, int id) =>
{
    var resultado = await repo.DeleteSector(id);

    return Results.Ok(resultado);
});
#endregion

app.Run();