using Evento.Core.Services.Enums;
using System.Text.Json.Serialization;


namespace Evento.Core.Entidades;

public class Usuario
{
    public int idUsuario { get; set; }
    public string Apodo { get; set; }
    public string Email { get; set; }
    public string Contrasena { get; set; }
    public Cliente cliente { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ERoles Role { get; set; } = ERoles.Usuario;

    public Usuario()
    { }
}
