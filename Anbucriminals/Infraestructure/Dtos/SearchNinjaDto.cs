using System.Runtime.Serialization;

namespace NarutoApi.Infrastructure.Soap.Dtos;

[DataContract(Name = "SearchNinjaDto", Namespace = "http://naruto-api/ninja-service")]
public class SearchNinjaDto
{
    // Filtros existentes
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

    // NUEVO: paginación + ordenamiento
    [DataMember(Name = "PageNumber", Order = 101)]
    public int PageNumber { get; set; } = 1;

    [DataMember(Name = "PageSize", Order = 102)]
    public int PageSize { get; set; } = 10;

    [DataMember(Name = "OrderBy", Order = 103)]
    public string OrderBy { get; set; } = "Name";  // Name | Village | Rank | Chakra | NinJutsu | CreatedAt (si aplica)

    [DataMember(Name = "OrderDirection", Order = 104)]
    public string OrderDirection { get; set; } = "asc"; // asc | desc
}
