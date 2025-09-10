using System.Runtime.Serialization;

namespace NarutoApi.Dtos;

[DataContract(Name = "SearchNinjaDto", Namespace = "http://naruto-api/ninja-service")]
public class SearchNinjaDto
{
    [DataMember(Name = "Village", Order = 1)]
    public string? Village { get; set; }

    [DataMember(Name = "Rank", Order = 2)]
    public string? Rank { get; set; }

    [DataMember(Name = "NinJutsu", Order = 3)]
    public string? NinJutsu { get; set; }

    [DataMember(Name = "ChakraMin", Order = 4)]
    public int? ChakraMin { get; set; }

    [DataMember(Name = "ChakraMax", Order = 5)]
    public int? ChakraMax { get; set; }
}
