using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NarutoApi.Models;

namespace NarutoApi.Repositories;

public interface INinjaRepository
{
   
    Task<Ninja?> GetByNameAsync(string name, CancellationToken cancellationToken);

    
    Task<IReadOnlyList<Ninja>> GetNinjasByNameAsync(string name, CancellationToken cancellationToken);

    
    Task<Ninja> CreateAsync(Ninja ninja, CancellationToken cancellationToken);
}
