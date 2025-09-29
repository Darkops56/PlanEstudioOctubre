using Evento.Core.Services.Repo;
using Evento.Core.Services.Validation;
using Evento.Dapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers()
    .AddFluentValidation(fv =>
    {
        fv.RegisterValidatorsFromAssemblyContaining<ClienteFluent>();
    });

builder.Services.AddScoped<IRepoEvento, RepoEvento>();
builder.Services.AddScoped<IRepoLocal, RepoLocal>();
builder.Services.AddScoped<IRepoCliente, RepoCliente>();
builder.Services.AddScoped<IRepoFuncion, RepoFuncion>();
builder.Services.AddScoped<IRepoTarifa, RepoTarifa>();
builder.Services.AddScoped<IRepoUsuario, RepoUsuario>();
builder.Services.AddScoped<IRepoEntrada, RepoEntrada>();
builder.Services.AddScoped<IRepoRefreshToken, RepoRefreshToken>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = "miapp",
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("olandesa51")),
            ValidateLifetime = true
        };
    });
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();