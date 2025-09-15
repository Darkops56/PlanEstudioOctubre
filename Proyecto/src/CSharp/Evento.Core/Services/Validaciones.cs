using System.Text.RegularExpressions;

namespace Evento.Core.Services;

public static class Validaciones
{
    public static string Cadena(string cadena)
    {
        if (string.IsNullOrEmpty(cadena))
            throw new ArgumentNullException("La cadena está vacia.");
        return cadena;
    }

    public static int Entero(int numero)
    {
        if (numero < 0)
            throw new ArgumentOutOfRangeException("El valor es inferior a 0");
        return numero;
    }
    public static string Email(string email)
    {
        string patron = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        if (!Regex.IsMatch(email, patron, RegexOptions.IgnoreCase))
            throw new Exception("El email no es valido");
        return email;
    }
    public static DateTime Fecha(DateTime Fecha)
    {
        if (Fecha > DateTime.Now)
            throw new ArgumentException("La fecha es invalida");
        return Fecha;
    }
}
