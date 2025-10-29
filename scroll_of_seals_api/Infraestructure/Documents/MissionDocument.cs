using MongoDB.Bson.Serialization.Attributes;

namespace ScrollsOfSealsApi.Infrastructure.Documents;

public class MissionDocument
{
    [BsonElement("village")]
    public string Village { get; set; } = null!; // Aldea que asignó la misión 

    [BsonElement("rank")]
    public MissionRankMongo Rank { get; set; }   
}
