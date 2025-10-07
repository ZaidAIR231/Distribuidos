using System.Runtime.Serialization;

namespace PokedexApi.Infrastructure.Soap.Dtos;

[DataContract(Name = "DeletePokemonResponseDto", Namespace = "http://pokemon-api/pokemon-service")]
public class DeletePokemonResponseDto
{
    [DataMember(Name = "Success", Order = 1)]
    public bool Success { get; set; }
}