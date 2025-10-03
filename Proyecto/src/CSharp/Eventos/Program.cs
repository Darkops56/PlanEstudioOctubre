using Evento.Core.Services.Repo;
using Evento.Core.Services.Utility;
using Evento.Core.Services.Validation;
using Evento.Dapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");
builder.Services.AddControllers()
    .AddFluentValidation(fv =>
    {
        fv.RegisterValidatorsFromAssemblyContaining<ClienteFluent>();
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { 
        Title = "Eventos API", 
        Version = "v1",
        Description = "API para sistema de gestión de entradas QR",
        Contact = new OpenApiContact
        {
            Name = "sisas Team",
            Email = "soporte@appqr.com"
        }
    });

    // Configuración de seguridad a JWT para Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header usando el esquema Bearer. Ejemplo: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
builder.Services.AddScoped<IAdo>(sp => new Ado(connectionString));
builder.Services.AddScoped<IRepoEvento, RepoEvento>();
builder.Services.AddScoped<IRepoOrdenCompra, RepoOrdenCompra>();
builder.Services.AddScoped<IRepoLocal, RepoLocal>();
builder.Services.AddScoped<IRepoCliente, RepoCliente>();
builder.Services.AddScoped<IRepoFuncion, RepoFuncion>();
builder.Services.AddScoped<IRepoTarifa, RepoTarifa>();
builder.Services.AddScoped<IRepoUsuario, RepoUsuario>();
builder.Services.AddScoped<IRepoEntrada, RepoEntrada>();
builder.Services.AddScoped<IRepoRefreshToken, RepoRefreshToken>();

//builder.Services.AddHostedService<StockExpiradoService>();
builder.WebHost.UseUrls("http://localhost:5002");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
            ValidateLifetime = true
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("v1/swagger.json", "Eventos API V1");
    c.RoutePrefix = "swagger"; // URL: /swagger
    c.DisplayRequestDuration(); // Muestra el tiempo de respuesta
    c.EnableDeepLinking(); // Permite enlaces directos a endpoints
    c.EnableTryItOutByDefault();
});


app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();