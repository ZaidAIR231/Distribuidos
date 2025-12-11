using System;
using Anbucriminals.Models;

namespace Anbucriminals.Dtos;

public sealed class PatchNinjaRequest
{
    public string? Name { get; init; }
    public string? Village { get; init; }
    public string? Rank { get; init; }
    public int? Chakra { get; init; }
    public string? NinJutsu { get; init; }
}

public static class PatchNinjaRequestExtensions
{
    // Aplica los cambios del patch sobre un Ninja existente (modelo inmutable con init-only → devolvemos uno nuevo)
    public static Ninja ApplyTo(this PatchNinjaRequest patch, Ninja original) => new()
    {
        Id       = original.Id,
        Name     = patch.Name     ?? original.Name,
        Village  = patch.Village  ?? original.Village,
        Rank     = patch.Rank     ?? original.Rank,
        Chakra   = patch.Chakra   ?? original.Chakra,
        NinJutsu = patch.NinJutsu ?? original.NinJutsu
    };
}
