namespace NarutoApi.Infrastructure.Entities;

public class NinjaEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Village { get; set; } = default!;
    public string Rank { get; set; } = default!;
    public int Chakra { get; set; }
    public string NinJutsu { get; set; } = default!;
}
