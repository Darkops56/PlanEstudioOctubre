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
        public async Task<IActionResult> Crear([FromBody] LocalDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var local = new Local
            {
                Nombre = dto.Nombre,
                Ubicacion = dto.Ubicacion
            };
            var id = await _repo.InsertLocal(local);
            return CreatedAtAction(nameof(ObtenerPorId), new { id }, local);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] LocalDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var local = new Local
            {
                idLocal = id,
                Ubicacion = dto.Ubicacion,
                Nombre = dto.Nombre,
            };
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
        public async Task<IActionResult> CrearSector(int id, [FromBody] SectorDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var local = await _repo.ObtenerPorId(id);

            var sector = new Sector
            {
                local = local,
                Capacidad = dto.Capacidad
            };
            var sectorId = await _repo.InsertSector(sector, id);
            return CreatedAtAction(nameof(ObtenerSectores), new { id }, sector);
        }

        [HttpPut("sectores/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ActualizarSector(int id, [FromBody] SectorDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var sector = await _repo.ObtenerSectorPorId(id);
            if (sector == null) return NotFound("No se encontró el Sector");

            sector.Capacidad = dto.Capacidad;
   
            var ok = await _repo.UpdateSector(sector, id);
            return ok ? NoContent() : NotFound();
        }

        [HttpDelete("sectores/{sectorId}")]
        public async Task<IActionResult> EliminarSector(int sectorId) =>
            (await _repo.DeleteSector(sectorId)) ? NoContent() : NotFound();
    }
}