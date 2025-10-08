using System.Collections.Generic;
using System.Runtime.Serialization;

namespace NarutoApi.Infrastructure.Soap.Dtos;

[DataContract(Name = "PagedNinjaResponseDto", Namespace = "http://naruto-api/ninja-service")]
public class PagedNinjaResponseDto
{
    [DataMember(Name = "Ninjas", Order = 1)]
    public List<NinjaResponseDto> Ninjas { get; set; } = new();

    [DataMember(Name = "TotalRecords", Order = 2)]
    public int TotalRecords { get; set; }
}
