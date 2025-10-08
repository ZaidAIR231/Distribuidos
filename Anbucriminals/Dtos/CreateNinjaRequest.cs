using Anbucriminals.Models;

namespace Anbucriminals.Dtos;

public sealed class CreateNinjaRequest
{
    public string Name { get; init; } = string.Empty;
    public string Village { get; init; } = string.Empty;
    public string Rank { get; init; } = string.Empty;
    public int Chakra { get; init; }
    public string NinJutsu { get; init; } = string.Empty;
}

public static class CreateNinjaRequestExtensions
{
    public static Ninja ToModel(this CreateNinjaRequest r) => new()
    {
        Id       = Guid.Empty, 
        Name     = r.Name,
        Village  = r.Village,
        Rank     = r.Rank,
        Chakra   = r.Chakra,
        NinJutsu = r.NinJutsu
    };
}
