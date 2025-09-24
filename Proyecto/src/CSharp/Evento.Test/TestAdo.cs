using Eventos.Core;
using Eventos.Dapper;

namespace Eventos.Test;

/// <summary>
/// Clase base para los tests. 
/// Se encarga de inicializar la instancia de IAdo con la conexión a la BD de Eventos.
/// </summary>
public class TestAdo
{
    protected readonly IAdo Ado;

    // ⚠️ Ajusta estos datos a tu configuración real de MySQL
    private const string _cadena = 
        "Server=localhost;Database=5to_Eventos;Uid=5to_agbd;Pwd=Trigg3rs!;Allow User Variables=True";

    public TestAdo()
    {
        Ado = new AdoDapper(_cadena);
    }

    public TestAdo(string cadena)
    {
        Ado = new AdoDapper(cadena);
    }
}
