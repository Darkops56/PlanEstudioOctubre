using Evento.Core.Entidades;
using MySql.Data.MySqlClient;
using Dapper;
using System;
using Evento.Core.Services;
using Evento.Dapper;

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
app.MapGet("/api/eventos", async (IRepoEvento repo) =>
{
    var eventos = await repo.ObtenerTodos();
    return eventos.Any() ? Results.Ok(eventos) : Results.NotFound();
});
app.MapGet("/api/clientes", async (IRepoCliente repo) =>
{
    var clientes = await repo.ObtenerTodos();
    return clientes.Any() ? Results.Ok(clientes) : Results.NotFound();
});

app.MapGet("/api/entradas", async (IRepoEntrada repo) =>
{
    var entradas = await repo.ObtenerTodos();
    return entradas.Any() ? Results.Ok()
});
#endregion
app.Run();
