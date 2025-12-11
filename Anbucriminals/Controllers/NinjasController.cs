using System;
using System.Threading;
using System.Threading.Tasks;
using Anbucriminals.Exceptions;
using Anbucriminals.Services;
using Microsoft.AspNetCore.Mvc;
using Anbucriminals.Dtos;

namespace Anbucriminals.Controllers;

[ApiController]
[Route("api/ninjas")]
public sealed class NinjasController(INinjaService service) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
    {
        try
        {
            var result = await service.GetByIdAsync(id, ct);
            return Ok(result);
        }
        catch (InvalidIdException ex)
        {
            return Problem(title: "Invalid id", detail: ex.Message, statusCode: StatusCodes.Status400BadRequest);
        }
        catch (NinjaNotFoundException ex)
        {
            return Problem(title: "Ninja not found", detail: ex.Message, statusCode: StatusCodes.Status404NotFound);
        }
        catch (UpstreamUnavailableException ex)
        {
            return Problem(title: "Upstream unavailable", detail: ex.Message, statusCode: StatusCodes.Status503ServiceUnavailable);
        }
    }
    [HttpPost]
    public async Task<ActionResult<NinjaResponse>> CreateNinjaAsync(
    [FromBody] CreateNinjaRequest createNinja,
    CancellationToken cancellationToken)
    {
        try
        {
            if (!IsValidNinja(createNinja))
            {
                return BadRequest(new { Message = "Invalid ninja payload" });
            }

            var created = await service.CreateNinjaAsync(createNinja.ToModel(), cancellationToken);

            // 201 Created → Location: /api/ninjas/{id}
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        catch (NinjaAlreadyExistsException e)
        {
            // 409 Conflict - { Message = "Ninja NAME already exists" }
            return Conflict(new { Message = e.Message });
        }

    }

    private static bool IsValidNinja(CreateNinjaRequest r)
        => !string.IsNullOrWhiteSpace(r.Name);
    // Http Verb - Put
    // Status:
    // 200 OK (si quisieras devolver el modificado)
    // 204 No Content (RESTful; sin body)
    // 400 Bad Request (payload inválido)
    // 404 Not Found (no existe)
    // 409 Conflict (nombre ya existe)
    // 503 Service Unavailable (SOAP caído)

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateNinjaAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateNinjaRequest ninja,
        CancellationToken cancellationToken)
    {
        try
        {
            if (!IsValidUpdate(ninja))
            {
                return BadRequest(new { Message = "Invalid ninja payload" });
            }

            await service.UpdateNinjaAsync(ninja.ToModel(id), cancellationToken);

            // 204 No Content
            return NoContent();
        }
        catch (NinjaNotFoundException)
        {
            return NotFound();
        }
        catch (NinjaAlreadyExistsException ex)
        {
            return Conflict(new { Message = ex.Message });
        }
    }

    private static bool IsValidUpdate(UpdateNinjaRequest r)
        => !string.IsNullOrWhiteSpace(r.Name);

    // Http Verb - Patch
    // Status:
    // 200 OK (recomendado aquí, devolviendo el recurso actualizado)
    // 204 No Content (alternativa válida)
    // 400 Bad Request (payload inválido)
    // 404 Not Found (no existe)
    // 409 Conflict (nombre ya existe)
    // 503 Service Unavailable (SOAP caído)

    [HttpPatch("{id:guid}")]
    public async Task<ActionResult<NinjaResponse>> PatchNinjaAsync(
        [FromRoute] Guid id,
        [FromBody] PatchNinjaRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            // Validación simple tipo "Attack > 0" de tu PokeAPI
            if (request.Chakra.HasValue && request.Chakra.Value < 0)
            {
                return BadRequest(new { Message = "Chakra must be non-negative" });
            }

            // Si manda Name vacío explícitamente
            if (request.Name is string n && string.IsNullOrWhiteSpace(n))
            {
                return BadRequest(new { Message = "Name cannot be empty" });
            }

            var updated = await service.PatchNinjaAsync(
                id,
                request.Name,
                request.Village,
                request.Rank,
                request.Chakra,
                request.NinJutsu,
                cancellationToken);

            return Ok(updated); // 200 con el ninja actualizado
        }
        catch (NinjaNotFoundException)
        {
            return NotFound(); // 404
        }
        catch (NinjaAlreadyExistsException ex)
        {
            return Conflict(new { Message = ex.Message }); // 409
        }
        catch (UpstreamUnavailableException ex)
        {
            return Problem(
                title: "Upstream unavailable",
                detail: ex.Message,
                statusCode: StatusCodes.Status503ServiceUnavailable);
        }
    }
    // HTTP Verb - DELETE
    // Status:
    // 204 No Content (borrado correcto)
    // 404 Not Found (no existe)
    // 503 Service Unavailable (SOAP caído)

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteNinjaAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await service.DeleteNinjaAsync(id, cancellationToken);
            return NoContent(); // 204
        }
        catch (NinjaNotFoundException)
        {
            return NotFound(); // 404
        }
        catch (UpstreamUnavailableException ex)
        {
            return Problem(
                title: "Upstream unavailable",
                detail: ex.Message,
                statusCode: StatusCodes.Status503ServiceUnavailable);
        }
    }
    [HttpGet]
public async Task<ActionResult<PagedResponse<NinjaResponse>>> GetNinjasAsync(
    [FromQuery] string? village,
    [FromQuery] string? rank,
    [FromQuery] string? ninJutsu,
    [FromQuery] int? chakraMin,
    [FromQuery] int? chakraMax,
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10,
    [FromQuery] string orderBy = "Name",
    [FromQuery] string orderDirection = "asc",
    CancellationToken cancellationToken = default)
{
    var page = await service.GetNinjasAsync(
        village, rank, ninJutsu, chakraMin, chakraMax,
        pageNumber, pageSize, orderBy, orderDirection,
        cancellationToken);

    return Ok(page);
}


}
