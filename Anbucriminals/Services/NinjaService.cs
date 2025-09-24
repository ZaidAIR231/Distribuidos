using System;
using System.Threading;
using System.Threading.Tasks;
using Anbucriminals.Dtos;
using Anbucriminals.Exceptions;
using Anbucriminals.Gateways;
using Anbucriminals.Mappers;

namespace Anbucriminals.Services;

public sealed class NinjaService(INinjaGateway gateway) : INinjaService
{
    private readonly INinjaGateway _gateway = gateway;

    public async Task<NinjaResponse> GetByIdAsync(Guid id, CancellationToken ct)
    {
        if (id == Guid.Empty) throw new InvalidIdException(id);

        var model = await _gateway.GetNinjaByIdAsync(id, ct);
        if (model is null)
            throw new NinjaNotFoundException(id);

        return NinjaMapper.ToResponse(model); 
    }
}
