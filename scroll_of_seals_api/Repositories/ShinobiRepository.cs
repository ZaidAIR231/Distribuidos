using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
using ScrollsOfSealsApi.Infrastructure;
using ScrollsOfSealsApi.Infrastructure.Documents;
using ScrollsOfSealsApi.Models;
using ScrollsOfSealsApi.Mappers;
using System.Linq;

namespace ScrollsOfSealsApi.Repositories
{
    public class ShinobiRepository : IShinobiRepository
    {
        private readonly IMongoCollection<ShinobiDocument> _shinobiCollection;

        public ShinobiRepository(IMongoDatabase database, IOptions<MongoDBSettings> settings)
        {
    
            var collectionName = settings.Value.ShinobiCollectionName;
            _shinobiCollection = database.GetCollection<ShinobiDocument>(collectionName);
        }

        public async Task<Shinobi?> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
         
            if (!ObjectId.TryParse(id, out var objectId))
            {
                return null;
            }

            var shinobi = await _shinobiCollection
                .Find(s => s.Id == objectId)
                .FirstOrDefaultAsync(cancellationToken);

            return shinobi?.ToDomain();
        }


        public async Task<Shinobi> CreateAsync(Shinobi shinobi, CancellationToken cancellationToken)
        {
            shinobi.CreatedAt = DateTime.UtcNow;

 
            var doc = shinobi.ToDocument();

            await _shinobiCollection.InsertOneAsync(doc, cancellationToken: cancellationToken);

            
            return doc.ToDomain();
        }

    
        public async Task<IEnumerable<Shinobi>> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Enumerable.Empty<Shinobi>();

           
            var filter = Builders<ShinobiDocument>.Filter.Regex(
                s => s.Name, new BsonRegularExpression(name, "i")
            );

            var docs = await _shinobiCollection
                .Find(filter)
                .ToListAsync(cancellationToken);

            return docs.Select(d => d.ToDomain());
        }
    }
}
