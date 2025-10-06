using Evento.Core.Entidades;
using Evento.Core.DTOs;
using Evento.Core.Services.Repo;
using Microsoft.AspNetCore.Mvc;
using Evento.Core.Services.Enums;

namespace Evento.Controllers
{
    [ApiController]
    [Route("api/entradas")]
    public class EntradasController : ControllerBase
    {
        private readonly IRepoEntrada _repoEntrada;
        private readonly IRepoTarifa _repoTarifa;
        private readonly IRepoOrdenCompra _repoOrden;
        private readonly IRepoQR _repoQR;

        public EntradasController(IRepoEntrada repo, IRepoTarifa repoTarifa, IRepoOrdenCompra repoOrdenCompra, IRepoQR repoQR)
        {
            _repoEntrada = repo;
            _repoTarifa = repoTarifa;
            _repoOrden = repoOrdenCompra;
            _repoQR = repoQR;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos() =>
            Ok(await _repoEntrada.ObtenerTodos());

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var entrada = await _repoEntrada.ObtenerEntrada(id);
            return entrada != null ? Ok(entrada) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] EntradaDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var tarifaObtenida = await _repoTarifa.ObtenerPorId(dto.idTarifa);
            if (tarifaObtenida?.funcion == null)
                return BadRequest("La tarifa o la función asociada no existen.");

            var ordencompra = await _repoOrden.ObtenerOrdenCompra(dto.idOrdenCompra);
            if (ordencompra?.usuario == null)
                return BadRequest("No se encontró la orden de compra o su usuario.");

            var entrada = new Entrada
            {
                tarifa = tarifaObtenida,
                ordenesCompra = ordencompra,
                PrecioPagado = dto.PrecioPagado,
                Estado = EEstados.Creado
            };

            var id = await _repoEntrada.InsertEntrada(entrada);
            return CreatedAtAction(nameof(ObtenerPorId), new { id }, entrada);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id) =>
            (await _repoEntrada.DeleteEntrada(id)) ? NoContent() : NotFound();

        [HttpPost("{id}/anular")]
        public async Task<IActionResult> Anular(int id)
        {
            var resultado = await _repoEntrada.AnularEntrada(id);
            return Ok(resultado);
        }

        // GET /entradas/{id}/qr  -> devuelve la imagen del QR
        [HttpGet("{id}/qr")]
        public async Task<IActionResult> ObtenerQrImagen(int id)
        {
            var qr = await _repoQR.ObtenerQRPorEntrada(id);
            if (qr == null) return NotFound("QR no encontrado");

            // Genera la imagen usando Skia/QrHelper
            var imageBytes = Evento.Core.Services.QrHelper.GenerarQrImageSkia(qr.url, pixelsPerModule: 8, quietZoneModules: 4);
            return File(imageBytes, "image/png");
        }

        [HttpPost("validar")]
        public async Task<IActionResult> ValidarQrPost([FromBody] QrDto dto)
        {
            if (dto == null) return BadRequest("Payload requerido");
            if (dto.entradaId <= 0) return BadRequest("entradaId inválido");
            if (string.IsNullOrWhiteSpace(dto.token)) return BadRequest("token requerido");

            var qr = await _repoQR.ObtenerQRPorEntrada(dto.entradaId);
            if (qr == null) return NotFound("QR no encontrado");

            if (qr.token != dto.token)
                return BadRequest("Token inválido");

            if (qr.ExpiraEn < DateTime.Now)
                return BadRequest("QR vencido");

            var entrada = await _repoEntrada.ObtenerEntrada(dto.entradaId);
            if (entrada == null) return NotFound("Entrada no encontrada");

            if (entrada.Estado == EEstados.Cancelado)
                return BadRequest("Entrada anulada");

            if (entrada.Estado == EEstados.Usado)
                return BadRequest("Entrada ya fue utilizada");

            await _repoEntrada.MarcarEntradaUsada(dto.entradaId);

            return Ok(new
            {
                mensaje = "QR válido",
                estado = "Usado",
                entradaId = dto.entradaId,
            });
        }

        // GET para que el escáner que abra la URL valide automáticamente
        [HttpGet("validar")]
        public async Task<IActionResult> ValidarQrGet([FromQuery] int entradaId, [FromQuery] string token)
        {
            return await ValidarQrPost(new QrDto { entradaId = entradaId, token = token });
        }
        [HttpGet("tokenQr")]
        public async Task<IActionResult> ObtenerQRToken([FromQuery] int entradaId)
        {
            var qr = await _repoQR.ObtenerQRPorEntrada(entradaId);
            return Ok(new
            {
                idEntrada = qr.idEntrada,
                token = qr.token,
            });
        }
    }
}
