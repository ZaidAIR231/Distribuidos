using ScrollsOfSealsApi.Models;

namespace ScrollsOfSealsApi.Repositories;

public interface IShinobiRepository
{
    Task<Shinobi?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<Shinobi> CreateAsync(Shinobi shinobi, CancellationToken cancellationToken);
    Task<IEnumerable<Shinobi>> GetByNameAsync(string name, CancellationToken cancellationToken);
}
