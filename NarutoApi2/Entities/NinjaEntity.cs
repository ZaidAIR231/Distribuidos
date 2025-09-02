namespace NarutoApi.Entities;

public class NinjaEntity
{
    public int Id { get; set; }                     // int autoincrement
    public string Name { get; set; } = string.Empty;
    public string Village { get; set; } = string.Empty; 
    public string Rank { get; set; } = string.Empty;    
    public int Chakra { get; set; }                 // 1 a 100
    public string MainJutsu { get; set; } = string.Empty;
}