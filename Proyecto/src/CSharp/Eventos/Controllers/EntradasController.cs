using Evento.Core.Entidades;
using Evento.Core.DTOs;
using Evento.Core.Services.Repo;
using Microsoft.AspNetCore.Mvc;
using Evento.Core.Services.Enums;
using Evento.Core.Services.Utility;

namespace Evento.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EntradasController : ControllerBase
    {
        private readonly IRepoEntrada _repoEntrada;
        private readonly IRepoTarifa _repoTarifa;
        private readonly IRepoOrdenCompra _repoOrden;

        public EntradasController(IRepoEntrada repo, IRepoTarifa repoTarifa, IRepoOrdenCompra repoOrdenCompra)
        {
            _repoEntrada = repo;
            _repoTarifa = repoTarifa;
            _repoOrden = repoOrdenCompra;
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

    }
}