using System;
using System.Runtime.Serialization;

namespace NarutoApi.Infrastructure.Soap.Dtos;

[DataContract(Name = "UpdateNinjaDto", Namespace = "http://naruto-api/ninja-service")]
public class UpdateNinjaDto
{
    [DataMember(Name = "Id", Order = 1)]
    public Guid Id { get; set; }

    [DataMember(Name = "Name", Order = 2)]
    public string Name { get; set; } = default!;

    [DataMember(Name = "Village", Order = 3)]
    public string Village { get; set; } = default!;

    [DataMember(Name = "Rank", Order = 4)]
    public string Rank { get; set; } = default!;

    [DataMember(Name = "Chakra", Order = 5)]
    public int Chakra { get; set; }

    [DataMember(Name = "NinJutsu", Order = 6)]
    public string NinJutsu { get; set; } = default!;
}
