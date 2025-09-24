using System.Runtime.Serialization;

namespace NarutoApi.Infrastructure.Soap.Dtos;

[DataContract(Name = "CreateNinjaDto", Namespace = "http://naruto-api/ninja-service")]
public class CreateNinjaDto
{
    [DataMember(Name = "Name", Order = 1)]
    public string Name { get; set; } = default!;

    [DataMember(Name = "Village", Order = 2)]
    public string Village { get; set; } = default!;

    [DataMember(Name = "Rank", Order = 3)]
    public string Rank { get; set; } = default!;

    [DataMember(Name = "Chakra", Order = 4)]
    public int Chakra { get; set; }

    [DataMember(Name = "NinJutsu", Order = 5)]   
    public string NinJutsu { get; set; } = default!;
}
