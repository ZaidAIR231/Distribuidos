using System.ServiceModel;
using NarutoApi.Dtos;
using System.Threading;

namespace NarutoApi.Services;

[ServiceContract(Name = "NinjaService", Namespace = "http://naruto-api/ninja-service")]
public interface INinjaService
{
    [OperationContract]
    Task<NinjaResponseDto> CreateNinja(CreateNinjaDto ninja, CancellationToken cancellationToken);
}
