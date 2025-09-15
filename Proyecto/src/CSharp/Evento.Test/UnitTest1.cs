namespace Eventos.Core.Entidades;

using Xunit;

public class TestAdoCliente : TestAdo
{
    [Theory]
    [InlineData(12345, "Juan Pérez", "123456")]
    [InlineData(67890, "Ana Gómez", "abcdef")]
    public void TraerCliente(int dni, string nombre, string pass)
    {
        var cliente = Ado.ClientePorPass(dni, pass);

        Assert.NotNull(cliente);
        Assert.Equal(nombre, cliente.nombreCompleto);
        Assert.Equal(dni, cliente.DNI);
    }

    [Theory]
    [InlineData(99999, "claveInvalida")]
    [InlineData(88888, "otraClave")]
    public void ClienteNoExiste(int dni, string pass)
    {
        var cliente = Ado.ClientePorPass(dni, pass);

        Assert.Null(cliente);
    }

    [Fact]
    public void AltaCliente()
    {
        int dni = 11111;
        string pass = "MiClaveSegura";
        string nombre = "Nuevo Cliente";
        string email = "nuevo@correo.com";

        // No debería existir al inicio
        var cliente = Ado.ClientePorPass(dni, pass);
        Assert.Null(cliente);

        // Damos de alta
        var nuevoCliente = new Cliente(dni, nombre, email, "123456789", pass);
        Ado.AltaCliente(nuevoCliente);

        // Ahora sí debería existir
        var mismoCliente = Ado.ClientePorPass(dni, pass);
        Assert.NotNull(mismoCliente);
        Assert.Equal(nombre, mismoCliente.nombreCompleto);
        Assert.Equal(email, mismoCliente.Email);
    }
}
