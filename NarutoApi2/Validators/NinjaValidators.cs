using System.ServiceModel;
using NarutoApi.Dtos;

namespace NarutoApi.Validators;

public static class NinjaValidators
{
    public static CreateNinjaDto ValidateName(this CreateNinjaDto dto) =>
        string.IsNullOrWhiteSpace(dto.Name)
            ? throw new FaultException("Ninja name is required")
            : dto;

    public static CreateNinjaDto ValidateVillage(this CreateNinjaDto dto) =>
        string.IsNullOrWhiteSpace(dto.Village)
            ? throw new FaultException("Ninja village is required")
            : dto;

    public static CreateNinjaDto ValidateRank(this CreateNinjaDto dto) =>
        string.IsNullOrWhiteSpace(dto.Rank)
            ? throw new FaultException("Ninja rank is required")
            : dto;

    public static CreateNinjaDto ValidateChakra(this CreateNinjaDto dto) =>
        (dto.Chakra < 1 || dto.Chakra > 100)
            ? throw new FaultException("Chakra must be between 1 and 100")
            : dto;

    public static CreateNinjaDto ValidateMainJutsu(this CreateNinjaDto dto) =>
        string.IsNullOrWhiteSpace(dto.MainJutsu)
            ? throw new FaultException("MainJutsu is required")
            : dto;
}
