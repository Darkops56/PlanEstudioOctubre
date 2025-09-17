namespace Evento.Core.Entidades;

public class Cliente
{
    public int DNI { get; set; }
    public string nombreCompleto { get; set; }
    public string Telefono { get; set; }

    public Cliente()
    {}
    
    //Métodos
    public void ActualizarContacto(string nuevoTelefono)
    {
        this.Telefono = nuevoTelefono;
    }
    public override string ToString()
    {
        return $"Cliente: {nombreCompleto} - DNI: {DNI}";
    }
}