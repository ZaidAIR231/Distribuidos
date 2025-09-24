using System;
using System.Threading;
using System.Threading.Tasks;
using Anbucriminals.Exceptions;
using Anbucriminals.Services;
using Microsoft.AspNetCore.Mvc;

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
}
