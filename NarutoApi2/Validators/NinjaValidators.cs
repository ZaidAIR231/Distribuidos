using System.ServiceModel;
using NarutoApi.Dtos;

namespace NarutoApi.Validators;

public static class NinjaValidators
{
    public static CreateNinjaDto ValidateName(this CreateNinjaDto ninja) =>
        string.IsNullOrWhiteSpace(ninja.Name)
            ? throw new FaultException("Ninja name is required")
            : ninja;

    public static CreateNinjaDto ValidateVillage(this CreateNinjaDto ninja) =>
        string.IsNullOrWhiteSpace(ninja.Village)
            ? throw new FaultException("Ninja village is required")
            : ninja;

    public static CreateNinjaDto ValidateRank(this CreateNinjaDto ninja) =>
        string.IsNullOrWhiteSpace(ninja.Rank)
            ? throw new FaultException("Ninja rank is required")
            : ninja;

    public static CreateNinjaDto ValidateNinJutsu(this CreateNinjaDto ninja) =>
        string.IsNullOrWhiteSpace(ninja.NinJutsu)
            ? throw new FaultException("NinJutsu is required")
            : ninja;

    public static CreateNinjaDto ValidateChakra(this CreateNinjaDto ninja) =>
        ninja.Chakra is < 1 or > 100
            ? throw new FaultException("Chakra must be between 1 and 100")
            : ninja;
}

