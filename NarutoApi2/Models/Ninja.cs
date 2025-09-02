namespace NarutoApi.Models;

public class Ninja
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Village { get; set; } = string.Empty;
    public string Rank { get; set; } = string.Empty;
    public int Chakra { get; set; }
    public string MainJutsu { get; set; } = string.Empty;
}