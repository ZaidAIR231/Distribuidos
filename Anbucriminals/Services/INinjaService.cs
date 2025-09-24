using System;
using System.Threading;
using System.Threading.Tasks;
using Anbucriminals.Dtos;

namespace Anbucriminals.Services;

public interface INinjaService
{
    Task<NinjaResponse> GetByIdAsync(Guid id, CancellationToken ct);
}
