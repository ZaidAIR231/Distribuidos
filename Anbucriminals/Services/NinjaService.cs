using System;
using System.Threading;
using System.Threading.Tasks;
using Anbucriminals.Dtos;
using Anbucriminals.Exceptions;
using Anbucriminals.Gateways;
using Anbucriminals.Mappers;
using Anbucriminals.Models;
using Anbucriminals.Services;

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
    public async Task<NinjaResponse> CreateNinjaAsync(Ninja ninja, CancellationToken ct)
    {
        if (ninja is null) throw new ArgumentNullException(nameof(ninja));
        if (string.IsNullOrWhiteSpace(ninja.Name))
            throw new ArgumentException("Name is required", nameof(ninja));

        var created = await _gateway.CreateNinjaAsync(ninja, ct);
        return NinjaMapper.ToResponse(created);
    }
    public async Task<NinjaResponse> UpdateNinjaAsync(Ninja ninja, CancellationToken ct)
    {
        if (ninja is null) throw new ArgumentNullException(nameof(ninja));
        if (ninja.Id == Guid.Empty) throw new InvalidIdException(ninja.Id);
        if (string.IsNullOrWhiteSpace(ninja.Name))
            throw new ArgumentException("Name is required", nameof(ninja));

        var updated = await _gateway.UpdateNinjaAsync(ninja, ct);
        return NinjaMapper.ToResponse(updated);
    }
    public async Task<NinjaResponse> PatchNinjaAsync(
        Guid id,
        string? name,
        string? village,
        string? rank,
        int? chakra,
        string? ninJutsu,
        CancellationToken ct)
    {
        if (id == Guid.Empty) throw new InvalidIdException(id);

        // 1) Traer el ninja
        var current = await _gateway.GetNinjaByIdAsync(id, ct);
        if (current is null) throw new NinjaNotFoundException(id);

        // 2) Merge (como properties son init-only, creamos uno nuevo)
        var patched = new Ninja
        {
            Id = current.Id,
            Name = name ?? current.Name,
            Village = village ?? current.Village,
            Rank = rank ?? current.Rank,
            Chakra = chakra ?? current.Chakra,
            NinJutsu = ninJutsu ?? current.NinJutsu
        };

        // 3) Persistir via SOAP (reusa Update)
        var updated = await _gateway.UpdateNinjaAsync(patched, ct);

        // 4) Devolver DTO
        return NinjaMapper.ToResponse(updated);
    }
    public async Task DeleteNinjaAsync(Guid id, CancellationToken ct)
    {
        if (id == Guid.Empty) throw new InvalidIdException(id);

        await _gateway.DeleteNinjaAsync(id, ct);
        // No retorna nada: el controlador devolverá 204
    }
 public async Task<PagedResponse<NinjaResponse>> GetNinjasAsync(
    string? village,
    string? rank,
    string? ninJutsu,
    int? chakraMin,
    int? chakraMax,
    int pageNumber,
    int pageSize,
    string orderBy,
    string orderDirection,
    CancellationToken ct)
{
    if (pageNumber <= 0) pageNumber = 1;
    if (pageSize   <= 0) pageSize   = 10;
    if (pageSize   > 100) pageSize  = 100;

    orderBy = string.IsNullOrWhiteSpace(orderBy) ? "Name" : orderBy;
    var desc = string.Equals(orderDirection, "desc", StringComparison.OrdinalIgnoreCase);

    var (models, totalRecords) = await _gateway.GetNinjasAsync(
        village, rank, ninJutsu, chakraMin, chakraMax,
        pageNumber, pageSize, orderBy, orderDirection, ct);

    // Ordenamiento dinámico seguro (sin reflexión)
    IEnumerable<Ninja> ordered = orderBy.ToLowerInvariant() switch
    {
        "name"     => desc ? models.OrderByDescending(x => x.Name)     : models.OrderBy(x => x.Name),
        "village"  => desc ? models.OrderByDescending(x => x.Village)  : models.OrderBy(x => x.Village),
        "rank"     => desc ? models.OrderByDescending(x => x.Rank)     : models.OrderBy(x => x.Rank),
        "chakra"   => desc ? models.OrderByDescending(x => x.Chakra)   : models.OrderBy(x => x.Chakra),
        "ninjutsu" => desc ? models.OrderByDescending(x => x.NinJutsu) : models.OrderBy(x => x.NinJutsu),
        _          => desc ? models.OrderByDescending(x => x.Name)     : models.OrderBy(x => x.Name),
    };

    // Paginación en memoria
    var paged = ordered
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .Select(NinjaMapper.ToResponse)
        .ToList();

    return PagedResponse<NinjaResponse>.Create(paged, totalRecords, pageNumber, pageSize);
}


   
}
