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
using Evento.Dapper.Middleware;
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


var rootConnection = "Server=localhost;Port=3306;User=root;Password=Darkops(1011);";
var appConnection = "Server=localhost;Port=3306;Database=5to_Eventos;User=administrador;Password=Admin123!;";



bool dbExists = false;
using (var conn = new MySqlConnection(rootConnection))
{
    conn.Open();
    using var cmd = new MySqlCommand($"SHOW DATABASES LIKE '{dbName}';", conn);
    using var reader = cmd.ExecuteReader();
    dbExists = reader.HasRows;
}

if (!dbExists)
{
    Console.WriteLine($"📦 Base de datos '{dbName}' no encontrada. Ejecutando scripts...");

    try
    {
        // Rutas de los scripts
        string basePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../scripts/bd/MySQL"));
        string userScriptPath = Path.Combine(basePath, "USER.sql");
        string ddlScriptPath = Path.Combine(basePath, "DDL.sql");
        string insertScriptPath = Path.Combine(basePath, "INSERT.sql");

        // === Ejecutar USER.sql con root ===
        if (File.Exists(userScriptPath))
        {
            Console.WriteLine("🧾 Ejecutando USER.sql...");
            ExecuteSqlScript(rootConnection, userScriptPath);
        }

        // === Ejecutar DDL.sql e INSERT.sql con el usuario de aplicación ===
        if (File.Exists(ddlScriptPath))
        {
            Console.WriteLine("🧱 Ejecutando DDL.sql...");
            ExecuteSqlScript(appConnection, ddlScriptPath);
        }

        if (File.Exists(insertScriptPath))
        {
            Console.WriteLine("📥 Ejecutando INSERT.sql...");
            ExecuteSqlScript(appConnection, insertScriptPath);
        }

        Console.WriteLine($"✅ Base de datos '{dbName}' creada exitosamente.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Error al ejecutar scripts: {ex.Message}");
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

app.UseMiddleware<MiddlewareErrorManage>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

static void ExecuteSqlScript(string connectionString, string filePath)
{
    string script = File.ReadAllText(filePath);
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
}