using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NarutoApi.Infrastructure;
using NarutoApi.Models;
using NarutoApi.Mappers;

namespace NarutoApi.Repositories;

public class NinjaRepository : INinjaRepository
{
    private readonly RelationalDbContext _context;

    public NinjaRepository(RelationalDbContext context)
    {
        _context = context;
    }

    public async Task<Ninja?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        var entity = await _context.Ninjas
            .AsNoTracking()
            .FirstOrDefaultAsync(n => n.Name == name, cancellationToken);

        return entity?.ToModel();
    }

    public async Task<IReadOnlyList<Ninja>> GetNinjasByNameAsync(string name, CancellationToken cancellationToken)
    {
        var entities = await _context.Ninjas
            .AsNoTracking()
            .Where(n => n.Name.Contains(name))
            .ToListAsync(cancellationToken);

        return entities.ToModel();
    }

    public async Task<Ninja> CreateAsync(Ninja ninja, CancellationToken cancellationToken)
    {
        var entity = ninja.ToEntity(); 
        await _context.Ninjas.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.ToModel();
    }
}