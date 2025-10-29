using Grpc.Core;
using System.Linq; 
using ScrollsOfSealsApi.Repositories;
using ScrollsOfSealsApi.Mappers;

namespace ScrollsOfSealsApi.Services;

public class ShinobiRegistryService : ShinobiRegistry.ShinobiRegistryBase
{
    private readonly IShinobiRepository _shinobiRepository;

    public ShinobiRegistryService(IShinobiRepository shinobiRepository)
    {
        _shinobiRepository = shinobiRepository;
    }

    public override async Task<ShinobiResponse> GetShinobiById(
        ShinobiByIdRequest request, 
        ServerCallContext context)
    {
        var shinobi = await _shinobiRepository.GetByIdAsync(request.Id, context.CancellationToken);
        if (shinobi is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"Shinobi with ID {request.Id} not found."));
        }

        return shinobi.ToResponse();
    }

    public override async Task<EnrollShinobiResponse> EnrollShinobi(
        IAsyncStreamReader<EnrollShinobiRequest> requestStream,
        ServerCallContext context)
    {
        var createdShinobi = new List<ShinobiResponse>();

        await foreach (var request in requestStream.ReadAllAsync(context.CancellationToken))
        {
            if (request is null) continue;

            var model = request.ToModel();

            var existing = await _shinobiRepository.GetByNameAsync(model.Name, context.CancellationToken);
            if (existing.Any())
            {
                // Ya existe, se salta
                continue;
            }

            var created = await _shinobiRepository.CreateAsync(model, context.CancellationToken);
            createdShinobi.Add(created.ToResponse());
        }

        return new EnrollShinobiResponse
        {
            SuccessCount = createdShinobi.Count,
            Shinobi = { createdShinobi },
        };
    }

    public override async Task FindShinobiByName(
        ShinobiByNameRequest request,
        IServerStreamWriter<ShinobiResponse> responseStream,
        ServerCallContext context)
    {
        var shinobiList = await _shinobiRepository.GetByNameAsync(
            name: request.Name,
            cancellationToken: context.CancellationToken);

        foreach (var shinobi in shinobiList)
        {
            if (context.CancellationToken.IsCancellationRequested)
                break;

            await responseStream.WriteAsync(shinobi.ToResponse());
            await Task.Delay(TimeSpan.FromSeconds(5), context.CancellationToken);
        }
    }
}
