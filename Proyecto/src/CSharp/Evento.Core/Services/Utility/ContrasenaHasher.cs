using Isopoh.Cryptography.Argon2;
namespace Evento.Core.Services.Security;

public static class ContrasenaHasher
{
    public static string Hash(string contrasena)
    {
        return Argon2.Hash(contrasena);
    }
    public static bool Verificar(string contrasena, string Hash)
    {
        return Argon2.Verify(contrasena, Hash);
    }
}
