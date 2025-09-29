using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Evento.Core.Entidades;
using Evento.Core.DTOs;
using Evento.Core.Services.Repo;


namespace Evento.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocalController : ControllerBase
    {
        private readonly IRepoLocal _repo;

        public LocalController(IRepoLocal repo) => _repo = repo;

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos() =>
            Ok(await _repo.ObtenerTodos());

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var local = await _repo.ObtenerPorId(id);
            return local != null ? Ok(local) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] Local local)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var id = await _repo.InsertLocal(local);
            return CreatedAtAction(nameof(ObtenerPorId), new { id }, local);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] Local local)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            local.idLocal = id;
            var ok = await _repo.UpdateLocal(local);
            return ok ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id) =>
            (await _repo.DeleteLocal(id)) ? NoContent() : NotFound();

        // Sectores
        [HttpGet("{id}/sectores")]
        public async Task<IActionResult> ObtenerSectores(int id) =>
            Ok(await _repo.ObtenerSectoresDelLocal(id));

        [HttpPost("{id}/sectores")]
        public async Task<IActionResult> CrearSector(int id, [FromBody] Sector sector)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var sectorId = await _repo.InsertSector(sector, id);
            return CreatedAtAction(nameof(ObtenerSectores), new { id }, sector);
        }

        [HttpPut("sectores/{sectorId}")]
        public async Task<IActionResult> ActualizarSector(int sectorId, [FromBody] Sector sector)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var ok = await _repo.UpdateSector(sector, sectorId);
            return ok ? NoContent() : NotFound();
        }

        [HttpDelete("sectores/{sectorId}")]
        public async Task<IActionResult> EliminarSector(int sectorId) =>
            (await _repo.DeleteSector(sectorId)) ? NoContent() : NotFound();
    }
}