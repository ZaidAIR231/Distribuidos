using System.Collections.Generic;
using System.Linq;
using NarutoApi.Dtos;
using NarutoApi.Entities;
using NarutoApi.Models;

namespace NarutoApi.Mappers
{
    public static class NinjaMappers
    {
        // ★ Necesario para compilar: DTO -> Model
        public static Ninja ToModel(this CreateNinjaDto dto) => new()
        {
            Name      = dto.Name,
            Village   = dto.Village,
            Rank      = dto.Rank,
            Chakra    = dto.Chakra,
            MainJutsu = dto.MainJutsu
        };

        // (lo que ya tenías)
        public static Ninja ToModel(this NinjaEntity e) => new()
        {
            Id        = e.Id,
            Name      = e.Name,
            Village   = e.Village,
            Rank      = e.Rank,
            Chakra    = e.Chakra,
            MainJutsu = e.MainJutsu
        };

        public static NinjaEntity ToEntity(this Ninja m) => new()
        {
            Id        = m.Id,
            Name      = m.Name,
            Village   = m.Village,
            Rank      = m.Rank,
            Chakra    = m.Chakra,
            MainJutsu = m.MainJutsu
        };

        public static NinjaResponseDto ToResponseDto(this Ninja m) => new()
        {
            Id        = m.Id,
            Name      = m.Name,
            Village   = m.Village,
            Rank      = m.Rank,
            Chakra    = m.Chakra,
            MainJutsu = m.MainJutsu
        };

        public static IReadOnlyList<Ninja> ToModel(this IReadOnlyList<NinjaEntity> entities)
            => entities.Select(e => e.ToModel()).ToList();

        public static IReadOnlyList<NinjaResponseDto> ToResponseDto(this IReadOnlyList<Ninja> models)
            => models.Select(m => m.ToResponseDto()).ToList();
    }
}
