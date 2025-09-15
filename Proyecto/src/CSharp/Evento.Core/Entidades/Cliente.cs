namespace Evento.Core.Entidades;

public class Cliente
{
    public int DNI { get; set; }
    public string nombreCompleto { get; set; }
    public string Email { get; set; }
    public string Telefono { get; set; }
    public string Contrasena { get; set; }

    public Cliente(int DNI, string nombreCompleto, string Email, string Telefono, string Contrasena)
    {
        this.DNI = DNI;
        this.nombreCompleto = nombreCompleto;
        this.Email = Email;
        this.Telefono = Telefono;
        this.Contrasena = Contrasena;
    }
    
    //Métodos
    public void ActualizarContacto(string nuevoEmail, string nuevoTelefono)
    {
        this.Email = nuevoEmail;
        this.Telefono = nuevoTelefono;
    }

    public bool VerificarContrasena(string contrasena)
    {
        return this.Contrasena == contrasena;
    }

    public override string ToString()
    {
        return $"Cliente: {nombreCompleto} - DNI: {DNI} - Email: {Email}";
    }
}