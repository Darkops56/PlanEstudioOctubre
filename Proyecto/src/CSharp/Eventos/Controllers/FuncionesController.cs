using Evento.Core.DTOs;
using Evento.Core.Entidades;
using Evento.Core.Services.Enums;
using Evento.Core.Services.Repo;
using Microsoft.AspNetCore.Mvc;

namespace Evento.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FuncionesController : ControllerBase
    {
        private readonly IRepoFuncion _repo;
        private readonly IRepoEvento _repoEvento;
        public FuncionesController(IRepoFuncion repo) => _repo = repo;

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] FuncionDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!Enum.TryParse<EEstados>(dto.Estado, true, out var estadoFuncion))
            {
                return BadRequest($"Estado '{dto.Estado}' no válido.");
            }

            var evento = await _repoEvento.ObtenerEventoPorId(dto.idEvento);
            if (evento == null)
                return NotFound("No se encontró el evento");

            var funcion = new Funcion
            {
                Estado = estadoFuncion,
                evento = evento,
                Fecha = dto.Fecha
            };
            var id = await _repo.InsertFuncion(funcion);
            return CreatedAtAction(nameof(ObtenerPorId), new { id }, funcion);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos() =>
            Ok(await _repo.ObtenerTodos());

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var funcion = await _repo.ObtenerPorId(id);
            return funcion != null ? Ok(funcion) : NotFound();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] Funcion funcion)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            funcion.idFuncion = id;
            var ok = await _repo.UpdateFuncion(funcion);
            return ok ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id) =>
            (await _repo.DeleteFuncion(id)) ? NoContent() : NotFound();

        [HttpGet("{id}/tarifas")]
        public async Task<IActionResult> ObtenerTarifas(int id) =>
            Ok(await _repo.ObtenerTarifasDeFuncion(id));

        [HttpPost("{id}/cancelar")]
        public async Task<IActionResult> Cancelar(int id)
        {
            var resultado = await _repo.CancelarFuncion(id);
            return Ok(resultado);
        }
    }
}