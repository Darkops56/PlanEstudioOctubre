using Evento.Core.Entidades;
using Evento.Core.Services.Repo;
using Microsoft.AspNetCore.Mvc;

namespace Evento.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TarifasController : ControllerBase
    {
        private readonly IRepoTarifa _repo;

        public TarifasController(IRepoTarifa repo) => _repo = repo;

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos() =>
            Ok(await _repo.ObtenerTodos());

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var tarifa = await _repo.ObtenerPorId(id);
            return tarifa != null ? Ok(tarifa) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] Tarifa tarifa)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var id = await _repo.InsertTarifa(tarifa);
            return CreatedAtAction(nameof(ObtenerPorId), new { id }, tarifa);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] Tarifa tarifa)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            tarifa.idTarifa = id;
            var ok = await _repo.UpdateTarifa(tarifa);
            return ok ? NoContent() : NotFound();
        }
    }
}