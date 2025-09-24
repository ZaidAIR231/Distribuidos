using System;

namespace Anbucriminals.Dtos;

public sealed class NinjaResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Village { get; init; } = string.Empty;
    public string Rank { get; init; } = string.Empty;
    public int Chakra { get; init; }
    public string NinJutsu { get; init; } = string.Empty;
}
