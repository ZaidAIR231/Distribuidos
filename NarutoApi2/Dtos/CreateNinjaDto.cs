using System.Runtime.Serialization;

namespace NarutoApi.Dtos;

[DataContract(Name = "CreateNinjaDto", Namespace = "http://naruto-api/ninja-service")]
public class CreateNinjaDto
{
    [DataMember(Name = "Name", Order = 1)]
    public required string Name { get; set; }

    [DataMember(Name = "Village", Order = 2)]
    public required string Village { get; set; } // Konoha, Suna, etc.

    [DataMember(Name = "Rank", Order = 3)]
    public required string Rank { get; set; } // Genin, Chunin, Jonin, Kage

    [DataMember(Name = "Chakra", Order = 4)]
    public int Chakra { get; set; } // 1..100

    [DataMember(Name = "MainJutsu", Order = 5)]
    public required string MainJutsu { get; set; }
}
