using Evento.Core.Entidades;
using Evento.Core.Services.Repo;
using Microsoft.AspNetCore.Mvc;

namespace Evento.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdenesCompraController : ControllerBase
    {
        private readonly IRepoOrdenCompra _repoOrden;

        public OrdenesCompraController(IRepoOrdenCompra repoOrden)
        {
            _repoOrden = repoOrden;
        }
        [HttpPost]
        public async Task<IActionResult> CrearOrden([FromBody] OrdenesCompra oc)
        {
            if (oc == null) return BadRequest("Debes enviar un cuerpo.");
            var id = await _repoOrden.InsertOrdenCompra(oc);
            return CreatedAtAction(nameof(ObtenerOrden), new { id = id }, oc);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerOrdenes()
        {
            var ordenes = await _repoOrden.ObtenerOrdenesCompra();
            return ordenes.Any() ? Ok(ordenes) : NotFound();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerOrden(int id)
        {
            var orden = await _repoOrden.ObtenerOrdenCompra(id);
            return orden != null ? Ok(orden) : NotFound();
        }

        [HttpPost("{id}/pagar")]
        public async Task<IActionResult> PagarOrden(int id)
        {
            var resultado = await _repoOrden.PagarOrdenCompra(id);
            if (!string.IsNullOrEmpty(resultado))
                return BadRequest(resultado);

            return Ok(new { mensaje = "Orden pagada correctamente" });
        }

        [HttpPost("{id}/cancelar")]
        public async Task<IActionResult> CancelarOrden(int id)
        {
            var resultado = await _repoOrden.CancelarOrdenCompra(id);
            if (!string.IsNullOrEmpty(resultado))
                return BadRequest(resultado);

            return Ok(new { mensaje = "Orden cancelada correctamente" });
        }
        [HttpPost("liberar-stock-expirado")]
        public async Task<IActionResult> LiberarStockExpirado()
        {
            var cantidadLiberada = await _repoOrden.LiberarStockExpirado();
            return Ok(new { mensaje = $"Se liberaron {cantidadLiberada} reservas expiradas." });
        }
    }
}