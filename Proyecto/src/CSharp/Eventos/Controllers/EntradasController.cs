using Evento.Core.Entidades;
using Evento.Core.Services.Repo;
using Microsoft.AspNetCore.Mvc;

namespace Evento.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EntradasController : ControllerBase
    {
        private readonly IRepoEntrada _repo;

        public EntradasController(IRepoEntrada repo) => _repo = repo;

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos() =>
            Ok(await _repo.ObtenerTodos());

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var entrada = await _repo.ObtenerEntrada(id);
            return entrada != null ? Ok(entrada) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] Entrada entrada)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var id = await _repo.InsertEntrada(entrada);
            return CreatedAtAction(nameof(ObtenerPorId), new { id }, entrada);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id) =>
            (await _repo.DeleteEntrada(id)) ? NoContent() : NotFound();

        [HttpPost("{id}/anular")]
        public async Task<IActionResult> Anular(int id)
        {
            var resultado = await _repo.AnularEntrada(id);
            return Ok(resultado);
        }

    }
}