using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Shared.Attributes;

namespace Shared.Models;

public class Project: BaseBsonModel
{
    [BsonElement("Name")] 
    public string Name { get; set; } = "";

    [BsonElement("SprintIds")] 
    [BsonCollection(typeof(Sprint))]
    public List<MongoDBRef> SprintIds { get; set; } = new();

    [BsonIgnore] 
    public List<Sprint> Sprints { get; set; } = new();
}