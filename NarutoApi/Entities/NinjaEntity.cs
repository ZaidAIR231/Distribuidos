namespace NarutoApi.Entities;

public class NinjaEntity
{
    public int Id { get; set; }                     // int autoincrement
    public string Name { get; set; } = string.Empty;
    public string Village { get; set; } = string.Empty; // Konoha, Suna, etc.
    public string Rank { get; set; } = string.Empty;    // Genin, Chunin, Jonin, Kage
    public int Chakra { get; set; }                 // 1..100
    public string MainJutsu { get; set; } = string.Empty;
}
