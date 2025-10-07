using PokedexApi.Infrastructure.Soap.Dtos;
namespace PokedexApi.Infrastructure.Soap.Dtos;
public class GetPokemonsRequestDto
{
    public string Name { get; set; }
    public string Type { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string OrderBy { get; set; }
    public string OrderDirection { get; set; }
}