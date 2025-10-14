using Evento.Core.Entidades;
using Evento.Core.Services.Repo;
using Microsoft.AspNetCore.Mvc;

namespace Evento.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly IRepoCliente _repo;

        public ClientesController(IRepoCliente repo) => _repo = repo;

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos() =>
            Ok(await _repo.ObtenerTodos());

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var cliente = await _repo.ObtenerPorId(id);
            return cliente != null ? Ok(cliente) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] Cliente cliente)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var id = await _repo.InsertCliente(cliente);
            return CreatedAtAction(nameof(ObtenerPorId), new { id }, cliente);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] Cliente cliente)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            cliente.DNI = id;
            var ok = await _repo.UpdateCliente(cliente);
            return ok ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id) =>
            (await _repo.DeleteCliente(id)) ? NoContent() : NotFound();

        [HttpGet("{id}/entradas")]
        public async Task<IActionResult> ObtenerEntradas(int id) =>
            Ok(await _repo.ObtenerEntradasPorCliente(id));
    }
}