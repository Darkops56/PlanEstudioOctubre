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

#region EndpointsEventos
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
app.MapPost("", async ([FromBody] Eventos evento, IRepoEvento repo) =>
{
    var nuevoEvento = await repo.InsertEvento(evento);
    if (nuevoEvento == null || nuevoEvento.ToString() == "")
    {
        return Results.BadRequest();
    }
    return Results.Created();
});

app.MapPut("/api/evento/{int:id}", async ([FromBody] int id, IRepoEvento repo) =>
{
    var evento = await repo.ObtenerEvento(id);
    await repo.UpdateEvento(evento);

    return Results.Ok(evento);
});
app.MapGet("/api/{int:id}/publicar", async ([FromBody] int id, IRepoEvento repo) =>
{
    var evento = await repo.ObtenerEvento(id);
});
app.MapGet("/api/{int:id}/cancelar", async ([FromBody] int id, IRepoEvento repo) =>
{
    var evento = await repo.ObtenerEvento(id);
});

#endregion

#region EndpointsClientes
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
    return Results.Ok(cliente);
});

#endregion

#region EndpointsEntradas
app.MapGet("/api/entradas", async (IRepoEntrada repo) =>
{
    var entradas = await repo.ObtenerTodos();
    return entradas.Any() ? Results.Ok() : Results.NotFound();
});
app.MapGet("/api/entrada/{int:id}", async ([FromBody] int id, IRepoEntrada repo) =>
{
    var entrada = await repo.ObtenerEntrada(id);
    if (entrada == null || entrada.ToString() == "")
    {
        return Results.NotFound();
    }
    return Results.Ok(entrada);
});
#endregion
app.Run();
