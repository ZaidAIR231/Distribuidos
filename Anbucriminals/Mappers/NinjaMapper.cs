using Anbucriminals.Dtos;
using Anbucriminals.Models;
using NarutoApi.Infrastructure.Soap.Dtos;

namespace Anbucriminals.Mappers;

public static class NinjaMapper
{
    public static Ninja FromSoap(NinjaResponseDto dto) => new()
    {
        Id = dto.Id,
        Name = dto.Name,
        Village = dto.Village,
        Rank = dto.Rank,
        Chakra = dto.Chakra,
        NinJutsu = dto.NinJutsu
    };

    public static NinjaResponse ToResponse(Ninja model) => new()
    {
        Id = model.Id,
        Name = model.Name,
        Village = model.Village,
        Rank = model.Rank,
        Chakra = model.Chakra,
        NinJutsu = model.NinJutsu
    };
    public static Ninja ToModel(this NinjaResponseDto dto) => new()
    {
        Id       = dto.Id,
        Name     = dto.Name,
        Village  = dto.Village,
        Rank     = dto.Rank,
        Chakra   = dto.Chakra,
        NinJutsu = dto.NinJutsu
    };

    public static IList<Ninja> ToModel(this IEnumerable<NinjaResponseDto> dtos)
        => dtos.Select(ToModel).ToList();

    public static CreateNinjaDto ToRequest(this Ninja model) => new()
    {
        Name     = model.Name,
        Village  = model.Village,
        Rank     = model.Rank,
        Chakra   = model.Chakra,
        NinJutsu = model.NinJutsu
    };
}

