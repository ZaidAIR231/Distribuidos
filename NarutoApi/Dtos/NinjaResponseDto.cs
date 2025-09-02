using System.Runtime.Serialization;

namespace NarutoApi.Dtos;

[DataContract(Name = "NinjaResponseDto", Namespace = "http://naruto-api/ninja-service")]
public class NinjaResponseDto
{
    [DataMember(Name = "Id", Order = 1)]
    public int Id { get; set; } // int autoincrement para que sea simple

    [DataMember(Name = "Name", Order = 2)]
    public required string Name { get; set; }

    [DataMember(Name = "Village", Order = 3)]
    public required string Village { get; set; }

    [DataMember(Name = "Rank", Order = 4)]
    public required string Rank { get; set; }

    [DataMember(Name = "Chakra", Order = 5)]
    public int Chakra { get; set; }

    [DataMember(Name = "MainJutsu", Order = 6)]
    public required string MainJutsu { get; set; }
}

