using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TrainerApi.Infrastructure;
using TrainerApi.Infrastructure.Documents;
using TrainerApi.Models;
using TrainerApi.Mappers;
using MongoDB.Bson;
using System.Linq; // para .Select

namespace TrainerApi.Repositories;

public class TrainerRepository : ITrainerRepository
{
    private readonly IMongoCollection<TrainerDocument> _trainerCollection;

    public TrainerRepository(IMongoDatabase database, IOptions<MongoDBSettings> settings)
    {
        _trainerCollection = database.GetCollection<TrainerDocument>(settings.Value.TrainerCollectionName);
    }

    public async Task<Trainer?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        var trainer = await _trainerCollection
            .Find(t => t.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

        return trainer?.ToDomain();
    }

    public async Task<Trainer> CreateAsync(Trainer trainer, CancellationToken cancellationToken)
    {
        trainer.CreatedAt = DateTime.UtcNow;

        // <<< AQUÍ estaba el error >>>
        var trainerToCreate = trainer.ToDocument(); // o: TrainerDocument trainerToCreate = trainer.ToDocument();

        await _trainerCollection.InsertOneAsync(trainerToCreate, cancellationToken: cancellationToken);

        return trainerToCreate.ToDomain();
    }

    public async Task<IEnumerable<Trainer>> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        var trainers = await _trainerCollection
            .Find(t => t.Name.Contains(name))
            .ToListAsync(cancellationToken);

        return trainers.Select(t => t.ToDomain());
    }
}
