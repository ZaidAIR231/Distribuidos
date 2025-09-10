using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using NarutoApi.Dtos;
using NarutoApi.Mappers;
using NarutoApi.Models;
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
        ninja.Validate();

        var duplicated = await _ninjaRepository.GetByNameAsync(ninja.Name, cancellationToken);
        if (duplicated is not null) throw new FaultException("Another ninja with the same name already exists");

        var model = ninja.ToModel();
        if (model.Id == Guid.Empty) model.Id = Guid.NewGuid();

        var created = await _ninjaRepository.CreateAsync(model, cancellationToken);
        return created.ToResponseDto();
    }

    public async Task<NinjaResponseDto> GetNinjaById(Guid id, CancellationToken cancellationToken)
    {
        var ninja = await _ninjaRepository.GetByIdAsync(id, cancellationToken);
        if (ninja is null) throw new FaultException(reason: "Ninja not found");
        return ninja.ToResponseDto();
    }

    public async Task<DeleteNinjaResponseDto> DeleteNinja(Guid id, CancellationToken cancellationToken)
    {
        var ninja = await _ninjaRepository.GetByIdAsync(id, cancellationToken);
        if (ninja is null) throw new FaultException(reason: "Ninja not found");

        await _ninjaRepository.DeleteAsync(ninja, cancellationToken);
        return new DeleteNinjaResponseDto { Success = true };
    }

 

    public async Task<NinjaResponseDto> UpdateNinja(UpdateNinjaDto ninjaToUpdate, CancellationToken cancellationToken)
    {
        ninjaToUpdate.Validate();

        var ninja = await _ninjaRepository.GetByIdAsync(ninjaToUpdate.Id, cancellationToken);
        if (ninja is null) throw new FaultException(reason: "Ninja not found");

        var duplicated = await _ninjaRepository.GetByNameAsync(ninjaToUpdate.Name, cancellationToken);
        if (duplicated is not null && duplicated.Id != ninjaToUpdate.Id)
            throw new FaultException("Another ninja with the same name already exists");

        ninja.Name     = ninjaToUpdate.Name;
        ninja.Village  = ninjaToUpdate.Village;
        ninja.Rank     = ninjaToUpdate.Rank;
        ninja.Chakra   = ninjaToUpdate.Chakra;
        ninja.NinJutsu = ninjaToUpdate.NinJutsu;

        await _ninjaRepository.UpdateNinjaAsync(ninja, cancellationToken);
        return ninja.ToResponseDto();
    }

    public async Task<IList<NinjaResponseDto>> GetNinjas(SearchNinjaDto filters, CancellationToken cancellationToken)
    {
        filters.Validate();

        var ninjas = await _ninjaRepository.SearchNinjasAsync(
            filters.Village, filters.Rank, filters.NinJutsu,
            filters.ChakraMin, filters.ChakraMax, cancellationToken);

        return ninjas.ToResponseDto();
    }
}
