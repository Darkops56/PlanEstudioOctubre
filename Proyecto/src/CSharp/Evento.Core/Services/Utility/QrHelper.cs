using QRCoder;
using SkiaSharp;
using System.Security.Cryptography;
using System.Text;

namespace Evento.Core.Services
{
    public static class QrHelper
    {
        private const string Base62 = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        // Token ligero (base62), por defecto 10 caracteres
        public static string GenerarToken(int length = 10)
        {
            var bytes = RandomNumberGenerator.GetBytes(length);
            var sb = new StringBuilder(length);
            for (int i = 0; i < bytes.Length; i++)
            {
                sb.Append(Base62[bytes[i] % Base62.Length]);
            }
            return sb.ToString();
        }

        // Construye la URL de validación
        // NOTA: usamos /qr/validar y query params, apuntando a http://localhost:5002
        public static string GenerarUrlValidacion(int entradaId, string token)
            => $"http://localhost:5002/entradas/validar?entradaId={entradaId}&token={token}";

        // Genera PNG bytes del QR usando QRCoder para obtener la matriz y SkiaSharp para render
        public static byte[] GenerarQrImageSkia(string url, int pixelsPerModule = 8, int quietZoneModules = 4)
        {
            // 1) generar data del QR
            using var qrGen = new QRCodeGenerator();
            using var qrData = qrGen.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);

            // qrData.ModuleMatrix es una lista de listas de bool (true = módulo oscuro)
            var matrix = qrData.ModuleMatrix;
            int moduleCount = matrix.Count;

            // 2) calcular tamaño final en px
            int size = (moduleCount + 2 * quietZoneModules) * pixelsPerModule;

            var info = new SKImageInfo(size, size, SKColorType.Bgra8888, SKAlphaType.Premul);
            using var bitmap = new SKBitmap(info);
            using var canvas = new SKCanvas(bitmap);

            // 3) pintar fondo blanco
            canvas.Clear(SKColors.White);

            // 4) preparación del pincel (negro) - sin antialias para píxeles nítidos
            using var paint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = SKColors.Black,
                IsAntialias = false
            };

            // 5) dibujar módulos
            for (int y = 0; y < moduleCount; y++)
            {
                for (int x = 0; x < moduleCount; x++)
                {
                    bool moduleOn = matrix[y][x]; // true si módulo oscuro
                    if (!moduleOn) continue;

                    int px = (quietZoneModules + x) * pixelsPerModule;
                    int py = (quietZoneModules + y) * pixelsPerModule;
                    var rect = new SKRectI(px, py, px + pixelsPerModule, py + pixelsPerModule);
                    canvas.DrawRect(rect, paint);
                }
            }

            // 6) flush y encode a PNG
            canvas.Flush();
            using var image = SKImage.FromBitmap(bitmap);
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);
            return data.ToArray();
        }
    }
}
