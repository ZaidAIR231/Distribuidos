using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using NarutoApi.Dtos;

namespace NarutoApi.Services;

[ServiceContract(Name = "NinjaService", Namespace = "http://naruto-api/ninja-service")]
public interface INinjaService
{
    [OperationContract]
    Task<NinjaResponseDto> CreateNinja(CreateNinjaDto ninja, CancellationToken cancellationToken);

    [OperationContract]
    Task<IReadOnlyList<NinjaResponseDto>> GetNinjasByName(string name, CancellationToken cancellationToken);
}
