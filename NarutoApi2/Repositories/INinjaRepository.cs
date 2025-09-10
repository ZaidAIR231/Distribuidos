using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NarutoApi.Models;

namespace NarutoApi.Repositories;

public interface INinjaRepository
{
    Task<Ninja?> GetByNameAsync(string name, CancellationToken cancellationToken);
    Task<IList<Ninja>> GetNinjasByNameAsync(string name, CancellationToken cancellationToken);

    Task<Ninja> CreateAsync(Ninja ninja, CancellationToken cancellationToken);
    Task<Ninja?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task DeleteAsync(Ninja ninja, CancellationToken cancellationToken);

   
    Task UpdateNinjaAsync(Ninja ninja, CancellationToken cancellationToken);

    Task<IList<Ninja>> SearchNinjasAsync(
        string? village,
        string? rank,
        string? ninJutsu,
        int? chakraMin,
        int? chakraMax,
        CancellationToken cancellationToken);
}


