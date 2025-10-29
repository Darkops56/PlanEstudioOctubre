using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Evento.Dapper.Middleware;

public class MiddlewareErrorManage
{
    private readonly RequestDelegate _next;

    public MiddlewareErrorManage(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (MySqlException ex)
        {
            context.Response.ContentType = "application/json";
            var payload = new
            {
                message = ex.Number == 1142 || ex.Number == 1044
                    ? "Acceso denegado. Tu usuario no tiene permisos para esta acción."
                    : "Error en la base de datos.",
                error = ex.Message,
                code = ex.Number
            };

            context.Response.StatusCode = (int)(
                ex.Number == 1142 || ex.Number == 1044
                    ? HttpStatusCode.Forbidden
                    : HttpStatusCode.InternalServerError
            );

            var json = JsonSerializer.Serialize(payload);
            await context.Response.WriteAsync(json);
        }
        catch (Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var payload = new
            {
                message = "Error interno del servidor.",
                error = ex.Message
            };

            var json = JsonSerializer.Serialize(payload);
            await context.Response.WriteAsync(json);
        }
    }
}
