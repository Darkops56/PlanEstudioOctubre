using Evento.Core.DTOs;
using Evento.Core.Entidades;
using Evento.Core.Services.Repo;
using Microsoft.AspNetCore.Mvc;

namespace Evento.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IRepoUsuario _repo;

        public UsuariosController(IRepoUsuario repo) => _repo = repo;

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos() =>
            Ok(await _repo.ObtenerTodos());

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var usuario = await _repo.ObtenerPorId(id);
            return usuario != null ? Ok(usuario) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] Usuario usuario)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var id = await _repo.InsertUsuario(usuario);
            return CreatedAtAction(nameof(ObtenerPorId), new { id }, usuario);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] Usuario usuario)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            usuario.idUsuario = id;
            var ok = await _repo.UpdateUsuario(usuario);
            return ok ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id) =>
            (await _repo.DeleteUsuario(id)) ? NoContent() : NotFound();

        [HttpGet("{id}/compras")]
        public async Task<IActionResult> ObtenerCompras(int id) =>
            Ok(await _repo.ObtenerComprasPorUsuario(id));

        // Login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var usuario = await _repo.Login(login.Email, login.Contrasena);
            if (usuario == null) return Unauthorized();

            // Generar JWT si es necesario (igual que tu implementación anterior)
            return Ok(usuario);
        }
    }
}