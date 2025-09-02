using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NarutoApi.Models;

namespace NarutoApi.Repositories;

public interface INinjaRepository
{
    // Para validación de duplicados (coincidencia exacta)
    Task<Ninja?> GetByNameAsync(string name, CancellationToken cancellationToken);

    // Para el endpoint de búsqueda (LIKE %name%)
    Task<IReadOnlyList<Ninja>> GetNinjasByNameAsync(string name, CancellationToken cancellationToken);

    // Creación
    Task<Ninja> CreateAsync(Ninja ninja, CancellationToken cancellationToken);
}
