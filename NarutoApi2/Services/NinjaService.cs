using System.ServiceModel;
using NarutoApi.Dtos;
using NarutoApi.Repositories;
using NarutoApi.Mappers;
using NarutoApi.Validators;
using NarutoApi.Models;

namespace NarutoApi.Services;

public class NinjaService : INinjaService
{
    private readonly INinjaRepository _ninjaRepository;

    public NinjaService(INinjaRepository ninjaRepository)
    {
        _ninjaRepository = ninjaRepository;
    }

    public async Task<NinjaResponseDto> CreateNinja(CreateNinjaDto request, CancellationToken cancellationToken)
    {
       
        request
            .ValidateName()
            .ValidateVillage()
            .ValidateRank()
            .ValidateNinJutsu()
            .ValidateChakra();

        if (await IsDuplicatedAsync(request.Name, cancellationToken))
            throw new FaultException("Ninja already exists");

        var ninja = await _ninjaRepository.CreateAsync(request.ToModel(), cancellationToken);
        return ninja.ToResponseDto();
    }

    public async Task<IList<NinjaResponseDto>> GetNinjasByName(string name, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new FaultException("Name is required");

        var ninjas = await _ninjaRepository.GetNinjasByNameAsync(name, cancellationToken);
        return ninjas.ToResponseDto();
    }

    private async Task<bool> IsDuplicatedAsync(string name, CancellationToken cancellationToken)
    {
        var existing = await _ninjaRepository.GetByNameAsync(name, cancellationToken);
        return existing is not null;
    }
}
