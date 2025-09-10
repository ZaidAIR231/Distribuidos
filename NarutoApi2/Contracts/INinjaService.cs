using System.ServiceModel;
using NarutoApi.Dtos;
using System.Threading;

namespace NarutoApi.Services;

[ServiceContract(Name = "NinjaService", Namespace = "http://naruto-api/ninja-service")]
public interface INinjaService
{
    [OperationContract]
    Task<NinjaResponseDto> CreateNinja(CreateNinjaDto ninja, CancellationToken cancellationToken);

    [OperationContract]
    Task<NinjaResponseDto> GetNinjaById(Guid id, CancellationToken cancellationToken);

    [OperationContract]
    Task<DeleteNinjaResponseDto> DeleteNinja(Guid id, CancellationToken cancellationToken);
    [OperationContract]
    Task<NinjaResponseDto> UpdateNinja(UpdateNinjaDto ninjaToUpdate, CancellationToken cancellationToken);

    [OperationContract]
    Task<IList<NinjaResponseDto>> GetNinjas(SearchNinjaDto filters, CancellationToken cancellationToken);

}
