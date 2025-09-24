namespace Evento.Core.Entidades;

public class Usuario
{
    public int idUsuario { get; set; }
    public string Apodo { get; set; }
    public string Email { get; set; }
    public string Contrasena { get; set; }
    public Cliente cliente { get; set; }
    public string Role { get; set; }

    public Usuario()
    { }

    public void ComprarEntrada()
    {
        
    }
}
