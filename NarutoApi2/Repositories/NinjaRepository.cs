using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NarutoApi.Infrastructure;
using NarutoApi.Mappers;
using NarutoApi.Models;

namespace NarutoApi.Repositories;

public class NinjaRepository : INinjaRepository
{
    private readonly RelationalDbContext _context;

    public NinjaRepository(RelationalDbContext context)
    {
        _context = context;
    }

    public async Task<Ninja?> GetByNameAsync(string name, CancellationToken cancellationToken)
        => (await _context.Ninjas.AsNoTracking()
               .FirstOrDefaultAsync(n => n.Name == name, cancellationToken))
           ?.ToModel();

    public async Task<Ninja?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => (await _context.Ninjas.AsNoTracking()
               .FirstOrDefaultAsync(n => n.Id == id, cancellationToken))
           ?.ToModel();

    public async Task DeleteAsync(Ninja ninja, CancellationToken cancellationToken)
    {
        _context.Ninjas.Remove(ninja.ToEntity());
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IList<Ninja>> GetNinjasByNameAsync(string name, CancellationToken cancellationToken)
        => (await _context.Ninjas.AsNoTracking()
                .Where(n => n.Name.Contains(name))
                .ToListAsync(cancellationToken))
           .ToModel();

    public async Task<Ninja> CreateAsync(Ninja ninja, CancellationToken cancellationToken)
    {
        var entity = ninja.ToEntity();
        await _context.Ninjas.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.ToModel();
    }

    public async Task UpdateNinjaAsync(Ninja ninja, CancellationToken cancellationToken)
    {
        _context.Ninjas.Update(ninja.ToEntity());
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IList<Ninja>> SearchNinjasAsync(
        string? village,
        string? rank,
        string? ninJutsu,
        int? chakraMin,
        int? chakraMax,
        CancellationToken cancellationToken)
    {
       
        var list = await _context.Ninjas.AsNoTracking()
            .Where(n =>
                (string.IsNullOrWhiteSpace(village)  || n.Village.Contains(village)) &&
                (string.IsNullOrWhiteSpace(rank)     || n.Rank.Contains(rank)) &&
                (string.IsNullOrWhiteSpace(ninJutsu) || n.NinJutsu.Contains(ninJutsu)) &&
                (!chakraMin.HasValue || n.Chakra >= chakraMin) &&
                (!chakraMax.HasValue || n.Chakra <= chakraMax))
            .ToListAsync(cancellationToken);

        return list.ToModel();
    }
}
