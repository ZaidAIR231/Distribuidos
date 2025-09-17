using System.Collections.Generic;
using System.Linq;
using NarutoApi.Dtos;
using NarutoApi.Infrastructure.Entities;
using NarutoApi.Models;

namespace NarutoApi.Mappers;

public static class NinjaMappers
{
    public static IList<NinjaResponseDto> ToResponseDto(this IEnumerable<Ninja> ninjas)
        => ninjas.Select(n => n.ToResponseDto()).ToList();

    public static IList<Ninja> ToModel(this IEnumerable<NinjaEntity> entities)
        => entities.Select(e => e.ToModel()).ToList();

    public static NinjaEntity ToEntity(this Ninja model) => new()
    {
        Id = model.Id,
        Name = model.Name,
        Village = model.Village,
        Rank = model.Rank,
        Chakra = model.Chakra,
        NinJutsu = model.NinJutsu
    };

    public static Ninja ToModel(this NinjaEntity entity) => new()
    {
        Id      = entity.Id,
        Name    = entity.Name,
        Village = entity.Village,
        Rank    = entity.Rank,
        Chakra  = entity.Chakra,
        NinJutsu= entity.NinJutsu
    };

    public static Ninja ToModel(this CreateNinjaDto dto) => new()
    {
        Name    = dto.Name,
        Village = dto.Village,
        Rank    = dto.Rank,
        Chakra  = dto.Chakra,
        NinJutsu= dto.NinJutsu
    };

    public static NinjaResponseDto ToResponseDto(this Ninja model) => new()
    {
        Id      = model.Id,
        Name    = model.Name,
        Village = model.Village,
        Rank    = model.Rank,
        Chakra  = model.Chakra,
        NinJutsu= model.NinJutsu
    };
}
