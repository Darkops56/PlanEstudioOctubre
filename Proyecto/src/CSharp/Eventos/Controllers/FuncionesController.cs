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
        private readonly ILogger<FuncionesController> _logger;
        public FuncionesController(IRepoFuncion repo, IRepoEvento repoEvento, ILogger<FuncionesController> logger)
        {
            _repo = repo;
            _repoEvento = repoEvento;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos() =>
            Ok(await _repo.ObtenerTodos());

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] FuncionDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!Enum.TryParse<EEstados>(dto.Estado, true, out var estadoFuncion))
                return BadRequest($"Estado '{dto.Estado}' no válido.");

            // Obtener evento
            var evento = await _repoEvento.ObtenerEventoPorId(dto.idEvento);

            // Validar null
            if (evento == null)
            {
                _logger.LogWarning("No se encontró el evento con id {IdEvento}", dto.idEvento);
                return NotFound($"No se encontró el evento con id {dto.idEvento}");
            }

            // Crear función
            var funcion = new Funcion
            {
                Estado = estadoFuncion,
                evento = evento,
                Fecha = dto.Fecha
            };

            _logger.LogInformation("Insertando Funcion con idEvento {IdEvento} y fecha {Fecha}", funcion.evento.idEvento, funcion.Fecha);

            var rows = await _repo.InsertFuncion(funcion);

            if (rows <= 0)
            {
                _logger.LogError("Error al insertar la función");
                return StatusCode(500, "Error al insertar la función");
            }

            return CreatedAtAction(nameof(ObtenerPorId), new { id = rows }, funcion);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var funcion = await _repo.ObtenerPorId(id);
            return funcion != null ? Ok(funcion) : NotFound();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] FuncionDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var estado = await _repo.ObtenerEstadoFuncion(dto.Estado);
            var evento = await _repoEvento.ObtenerEventoPorId(dto.idEvento);
            var funcion = new Funcion
            {
                idFuncion = id,
                Fecha = dto.Fecha,
                Estado = estado,
                evento = evento
            };
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