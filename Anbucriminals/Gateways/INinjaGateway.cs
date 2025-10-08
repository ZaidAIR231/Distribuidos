using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Anbucriminals.Models;
using NarutoApi.Infraestructure.SOAP.Contracts;
using NarutoApi.Infrastructure.Soap.Dtos;



namespace Anbucriminals.Gateways;

public interface INinjaGateway
{
    Task<Ninja?> GetNinjaByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Ninja> CreateNinjaAsync(Ninja ninja, CancellationToken cancellationToken);
    Task<Ninja> UpdateNinjaAsync(Ninja ninja, CancellationToken cancellationToken);
    Task DeleteNinjaAsync(Guid id, CancellationToken ct);
    
    Task<(IList<Ninja> ninjas, int totalRecords)> GetNinjasAsync(
    string? village,
    string? rank,
    string? ninJutsu,
    int? chakraMin,
    int? chakraMax,
    int pageNumber,
    int pageSize,
    string orderBy,
    string orderDirection,
    CancellationToken cancellationToken);

}
