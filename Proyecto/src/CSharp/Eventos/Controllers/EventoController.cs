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
    public class EventoController : ControllerBase
    {
        private readonly IRepoEvento _repo;

    public EventoController(IRepoEvento repo) => _repo = repo;

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos() =>
        Ok(await _repo.ObtenerTodos());

    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var evento = await _repo.ObtenerEventoPorId(id);
        return evento != null ? Ok(evento) : NotFound(new ProblemDetails
        {
            Title = "Evento no encontrado",
            Detail = $"No existe un evento con id {id}",
            Status = 404
        });
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] EventoDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var tipoDto = await _repo.ObtenerTipoEventoPorNombre(dto.tipoEvento.ToLower().Trim());
        if (tipoDto == null) return NotFound("No se encontro el tipo del evento.");

        var tipo = new TipoEvento
        {
            tipoEvento = UniqueFormatStrings.NormalizarString(tipoDto.tipoEvento.ToString()),
            idTipoEvento = tipoDto.idTipoEvento
        };
        var evento = new Eventos
        {
            Nombre = dto.Nombre,
            idTipoEvento = tipo.idTipoEvento,
            tipoEvento = tipo,
            EstadoEvento = EEstados.Pendiente,
            fechaFin = dto.fechaFin,
            fechaInicio = dto.fechaInicio
        };
        var id = await _repo.InsertEvento(evento);
        return CreatedAtAction(nameof(ObtenerPorId), new { id }, evento);
    }
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Actualizar(int id, [FromBody] EventoDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var tipoDto = await _repo.ObtenerTipoEventoPorNombre(dto.tipoEvento);
        var tipo = new TipoEvento
        {
            tipoEvento = tipoDto.tipoEvento.ToString(),
            idTipoEvento = tipoDto.idTipoEvento
        };
        var evento = new Eventos
        {
            idTipoEvento = tipo.idTipoEvento,
            tipoEvento = tipo,
            fechaFin = dto.fechaFin,
            fechaInicio = dto.fechaInicio,
            Nombre = dto.Nombre
        };

        evento.idEvento = id;
        var ok = await _repo.UpdateEvento(evento);
        return ok ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Eliminar(int id) =>
        (await _repo.DeleteEvento(id)) ? NoContent() : NotFound();

    [HttpPost("{id}/publicar")]
    public async Task<IActionResult> Publicar(int id)
    {
        var resultado = await _repo.PublicarEvento(id);
        return Ok(resultado);
    }

    [HttpPost("{id}/cancelar")]
    public async Task<IActionResult> Cancelar(int id)
    {
        var resultado = await _repo.CancelarEvento(id);
        return Ok(resultado);
    }

    [HttpGet("{id}/funciones")]
    public async Task<IActionResult> ObtenerFunciones(int id) =>
        Ok(await _repo.ObtenerFuncionesPorEventoAsync(id));
    }
}