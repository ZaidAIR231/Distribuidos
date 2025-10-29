// Infrastructure/Documents/ShinobiDocument.cs
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ScrollsOfSealsApi.Infrastructure.Documents;

public class ShinobiDocument
{
    [BsonId]
    public ObjectId Id { get; set; }                   

    [BsonElement("name")]
    public string Name { get; set; } = null!;

    [BsonElement("age")]
    public int Age { get; set; }

    [BsonElement("birthdate")]
    public DateTime Birthdate { get; set; }

    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; }

    [BsonElement("village")]
    public string Village { get; set; } = null!;

    [BsonElement("missions")]
    public List<MissionDocument> Missions { get; set; } = new();
}
