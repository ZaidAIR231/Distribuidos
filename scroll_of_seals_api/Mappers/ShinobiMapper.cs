using MongoDB.Bson;
using ScrollsOfSealsApi.Infrastructure.Documents;
using ScrollsOfSealsApi.Models;
using Google.Protobuf.WellKnownTypes;
using System.Linq;

namespace ScrollsOfSealsApi.Mappers;

public static class ShinobiMapper
{
    // Document to Domain
    public static Shinobi ToDomain(this ShinobiDocument document)
    {
        if (document == null) return null!;
        return new Shinobi
        {
            Id = document.Id.ToString(),          
            Name = document.Name,
            Age = document.Age,
            Birthdate = document.Birthdate,
            CreatedAt = document.CreatedAt,
            Village = document.Village,
            Missions = document.Missions?.Select(m => m.ToDomain()).ToList() ?? new()
        };
    }

    public static Models.Mission ToDomain(this MissionDocument document)
    {
        if (document == null) return null!;
        return new Models.Mission
        {
            Village = document.Village,
            Rank = (Models.MissionRank)(int)document.Rank
        };
    }

  
    public static ShinobiResponse ToResponse(this Shinobi shinobi)
    {
      
        var id = string.IsNullOrWhiteSpace(shinobi.Id) ? string.Empty : shinobi.Id;

        return new ShinobiResponse
        {
            Id = id, 
            Name = shinobi.Name,
            Age = shinobi.Age,
            Birthdate = Timestamp.FromDateTime(shinobi.Birthdate.ToUniversalTime()),
            CreatedAt = Timestamp.FromDateTime(shinobi.CreatedAt.ToUniversalTime()),
            Village = shinobi.Village,
            Missions = { shinobi.Missions.Select(m => m.ToResponse()) }
        };
    }

    private static Mission ToResponse(this Models.Mission mission)
    {
        return new Mission
        {
            Village = mission.Village,
            Rank = (MissionRank)(int)mission.Rank
        };
    }

    // Proto Request 
    private static Models.Mission ToModel(this Mission missionReq)
    {
        return new Models.Mission
        {
            Village = missionReq.Village,
            Rank = (Models.MissionRank)(int)missionReq.Rank
        };
    }

    public static Shinobi ToModel(this EnrollShinobiRequest request)
    {
        return new Shinobi
        {
            
            Name = request.Name,
            Age = request.Age,
            Birthdate = request.Birthdate.ToDateTime(),
            Village = request.Village,
            Missions = request.Missions.Select(m => m.ToModel()).ToList()
        };
    }

 
    private static MissionDocument ToDocument(this Models.Mission mission)
    {
        return new MissionDocument
        {
            Village = mission.Village,
            Rank = (MissionRankMongo)mission.Rank
        };
    }

    public static ShinobiDocument ToDocument(this Shinobi shinobi)
    {
        return new ShinobiDocument
        {
          
            Name = shinobi.Name,
            Age = shinobi.Age,
            Birthdate = shinobi.Birthdate,
            CreatedAt = shinobi.CreatedAt,
            Village = shinobi.Village,
            Missions = shinobi.Missions.Select(m => m.ToDocument()).ToList()
        };
    }
}
