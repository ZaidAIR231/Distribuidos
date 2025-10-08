using System;
using System.Threading;
using System.Threading.Tasks;
using Anbucriminals.Dtos;
using Anbucriminals.Models;
using Anbucriminals.Exceptions;


namespace Anbucriminals.Services;

public interface INinjaService
{
  Task<NinjaResponse> GetByIdAsync(Guid id, CancellationToken ct);
  Task<NinjaResponse> CreateNinjaAsync(Ninja ninja, CancellationToken ct);
  Task<NinjaResponse> UpdateNinjaAsync(Ninja ninja, CancellationToken ct);

  Task<NinjaResponse> PatchNinjaAsync(
    Guid id,
    string? name,
    string? village,
    string? rank,
    int? chakra,
    string? ninJutsu,
    CancellationToken ct);
  Task DeleteNinjaAsync(Guid id, CancellationToken ct);

  Task<PagedResponse<NinjaResponse>> GetNinjasAsync(
    string? village,
    string? rank,
    string? ninJutsu,
    int? chakraMin,
    int? chakraMax,
    int pageNumber,
    int pageSize,
    string orderBy,
    string orderDirection,
    CancellationToken ct);


}

