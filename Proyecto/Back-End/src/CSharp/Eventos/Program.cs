using Evento.Core.Services.Repo;
using Evento.Core.Services.Utility;
using Evento.Core.Services.Validation;
using Evento.Dapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using MySql.Data.MySqlClient;
using System.IO;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var activeUserKey = builder.Configuration["ActiveDatabaseUser"] ?? "MySqlAdminConnection";
var connectionString = builder.Configuration.GetConnectionString(activeUserKey)
    ?? throw new Exception($"No se encontró la cadena de conexión '{activeUserKey}' en appsettings.json.");

builder.Services.AddControllers()
    .AddFluentValidation(fv =>
    {
        fv.RegisterValidatorsFromAssembly(Assembly.Load("Evento.Core"));
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Eventos API",
        Version = "v1",
        Description = "API para sistema de gestión de entradas QR",
        Contact = new OpenApiContact
        {
            Name = "sisas Team",
            Email = "soporte@appqr.com"
        }
    });

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
builder.Services.AddScoped<IRepoQR, RepoQR>();

if (connectionString == "Server=localhost;Port=3305;Database=5to_Eventos;User=administrador;Password=Admin123!;" || connectionString == "Server=localhost;Port=3305;Database=5to_Eventos;User=root;Password=Darkops(1011);")
{
    builder.Services.AddHostedService<StockExpiradoService>();
}

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


var dbName = "5to_Eventos";

bool databaseExists = false;

using (var conn = new MySqlConnection(connectionString))
{
    try
    {
        conn.Open();
        using var cmd = new MySqlCommand($"SHOW DATABASES LIKE '{dbName}';", conn);
        using var reader = cmd.ExecuteReader();
        databaseExists = reader.HasRows;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"⚠️ Error al verificar base de datos: {ex.Message}");
    }
}

if (!databaseExists)
{
    Console.WriteLine($"📦 Base de datos '{dbName}' no encontrada. Ejecutando install.sql...");

    try
    {
        var installPath = Path.Combine(AppContext.BaseDirectory, "install.sql");

        if (!File.Exists(installPath))
        {
            Console.WriteLine("❌ No se encontró el archivo install.sql en la carpeta del ejecutable.");
        }
        else
        {
            string script = File.ReadAllText(installPath);

            // Dividir en comandos individuales por ';'
            var commands = script.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            using var conn = new MySqlConnection(connectionString);
            conn.Open();

            foreach (var command in commands)
            {
                string trimmed = command.Trim();
                if (string.IsNullOrWhiteSpace(trimmed) || trimmed.StartsWith("--"))
                    continue;

                using var cmd = new MySqlCommand(trimmed, conn);
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine($"✅ Base de datos '{dbName}' creada exitosamente.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Error al ejecutar install.sql: {ex.Message}");
    }
}
else
{
    Console.WriteLine($"✅ Base de datos '{dbName}' ya existente. Continuando con el arranque...");
}

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("v1/swagger.json", "Eventos API V1");
    c.RoutePrefix = "swagger";
    c.DisplayRequestDuration(); 
    c.EnableDeepLinking(); 
    c.EnableTryItOutByDefault();
});

app.UseRouting();

app.UseCors("AllowReactApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();