using NarutoApi.Models;

namespace NarutoApi.Repositories;

public interface INinjaRepository
{
    Task<Ninja?> GetByNameAsync(string name, CancellationToken cancellationToken);

    Task<IReadOnlyList<Ninja>> GetNinjasByNameAsync(string name, CancellationToken cancellationToken);

    Task<Ninja> CreateAsync(Ninja ninja, CancellationToken cancellationToken);
    Task<Ninja?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task DeleteAsync(Ninja ninja, CancellationToken cancellationToken);

}

