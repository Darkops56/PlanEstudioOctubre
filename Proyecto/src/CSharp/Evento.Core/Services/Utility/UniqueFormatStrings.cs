namespace Evento.Core.Services.Utility
{
    public static class UniqueFormatStrings
    {
        public static string NormalizarString(string cadena)
        {
            return cadena.ToLower().Trim();
        }
    }
}