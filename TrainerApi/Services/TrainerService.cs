using Grpc.Core;
using System.Linq; // para .Any()
using TrainerApi.Repositories;
using TrainerApi.Mappers;

namespace TrainerApi.Services;

public class TrainerService : TrainerApi.TrainerService.TrainerServiceBase
{
    private readonly ITrainerRepository _trainerRepository;

    public TrainerService(ITrainerRepository trainerRepository)
    {
        _trainerRepository = trainerRepository;
    }

    public override async Task<TrainerResponse> GetTrainerById(TrainerByIdRequest request, ServerCallContext context)
    {
        var trainer = await _trainerRepository.GetByIdAsync(request.Id, context.CancellationToken);
        if (trainer is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"Trainer with ID {request.Id} not found."));
        }

        return trainer.ToResponse();
    }

    public override async Task<CreateTrainerResponse> CreateTrainers(
        IAsyncStreamReader<CreateTrainerRequest> requestStream,
        ServerCallContext context)
    {
        var createdTrainers = new List<TrainerResponse>();

        while (await requestStream.MoveNext(context.CancellationToken))
        {
            var request = requestStream.Current; // ✅ declaración correcta
            if (request is null) continue;

            var trainer = request.ToModel();

            var trainerExists = await _trainerRepository.GetByNameAsync(trainer.Name, context.CancellationToken);
            if (trainerExists.Any())
            {
                // ya existe, saltamos
                continue;
            }

            var createdTrainer = await _trainerRepository.CreateAsync(trainer, context.CancellationToken);
            createdTrainers.Add(createdTrainer.ToResponse());
        }

        return new CreateTrainerResponse
        {
            SuccessCount = createdTrainers.Count,
            Trainers = { createdTrainers },
        };
    }

    public override async Task GetAllTrainersByName(
        TrainersByNameRequest request,
        IServerStreamWriter<TrainerResponse> responseStream,
        ServerCallContext context)
    {
        var trainers = await _trainerRepository.GetByNameAsync(
            name: request.Name,
            cancellationToken: context.CancellationToken);

        foreach (var trainer in trainers)
        {
            if (context.CancellationToken.IsCancellationRequested)
                break;

            await responseStream.WriteAsync(trainer.ToResponse());
            await Task.Delay(TimeSpan.FromSeconds(5), context.CancellationToken);
        }
    }

        
        
}
