using Isopoh.Cryptography.Argon2;
namespace Evento.Core.Services.Security;

public static class ContrasenaHasher
{
    public static string Hash(string contrasena)
    {
        return Argon2.Hash(contrasena, 
            timeCost: 2,
            memoryCost: 16384,
            parallelism: 1,  
            hashLength: 32);
    }
    public static bool Verificar(string contrasena, string Hash)
    {
        return Argon2.Verify(Hash, contrasena);
    }
}
