using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using NarutoApi.Dtos;
using NarutoApi.Mappers;
using NarutoApi.Repositories;
using NarutoApi.Validators;

namespace NarutoApi.Services;

public class NinjaService : INinjaService
{
    private readonly INinjaRepository _ninjaRepository;

    public NinjaService(INinjaRepository ninjaRepository)
    {
        _ninjaRepository = ninjaRepository;
    }

    public async Task<NinjaResponseDto> CreateNinja(CreateNinjaDto ninja, CancellationToken cancellationToken)
    {
        // Validaciones (fluent)
        ninja
            .ValidateName()
            .ValidateVillage()
            .ValidateRank()
            .ValidateChakra()
            .ValidateMainJutsu();

        // No duplicados por nombre exacto
        var existing = await _ninjaRepository.GetByNameAsync(ninja.Name, cancellationToken);
        if (existing is not null)
            throw new FaultException("Ninja already exists");

        var created = await _ninjaRepository.CreateAsync(ninja.ToModel(), cancellationToken);
        return created.ToResponseDto();
    }

    public async Task<IReadOnlyList<NinjaResponseDto>> GetNinjasByName(string name, CancellationToken cancellationToken)
    {
        var ninjas = await _ninjaRepository.GetNinjasByNameAsync(name, cancellationToken);
        return ninjas.ToResponseDto();
    }
}
