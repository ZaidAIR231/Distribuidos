using System.Runtime.Serialization;
namespace NarutoApi.Infrastructure.Soap.Dtos;

[DataContract(Name = "DeleteNinjaResponseDto", Namespace = "http://naruto-api/ninja-service")]
public class DeleteNinjaResponseDto
{
    [DataMember(Name = "Success", Order = 1)]
    public bool Success { get; set; }
}
