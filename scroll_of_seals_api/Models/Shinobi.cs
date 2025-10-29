namespace ScrollsOfSealsApi.Models;

public class Shinobi
{
    public string Id { get; set; } = null!;              
    public string Name { get; set; } = null!;             
    public int Age { get; set; }                          
    public DateTime Birthdate { get; set; }               // Fecha de nacimiento
    public DateTime CreatedAt { get; set; }               // Fecha en que fue registrado en el Libro Bingo

    public string Village { get; set; } = null!;          // Aldea 

    public List<Mission> Missions { get; set; } = new();  // Misiones completadas y su rango
}

public class Mission
{
    public string Village { get; set; } = null!;          // Aldea que asignó la misión
    public MissionRank Rank { get; set; }                 // Nivel de la misión
}
public enum MissionRank
{
    Unspecified = 0,   // Lo mismo que desconocido porque no lo escribo bien jaja
    D = 1,             // Misión sencilla (buscar un gato, limpiar un río)
    C = 2,             // Misión de bajo riesgo (protección local)
    B = 3,             // Misión media (escolta o combate limitado)
    A = 4,             // Alta dificultad (espionaje, captura)
    S = 5              // Nivel legendario (amenaza de rango Kage)
}
