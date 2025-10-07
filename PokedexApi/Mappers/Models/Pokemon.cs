namespace PokedexApi.Models;

public class Pokemon
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public int Level { get; set; }
    public Stats Stats { get; set; }
}

public class Stats
{
    public int HP { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int Speed { get; set; }
}