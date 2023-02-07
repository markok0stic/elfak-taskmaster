using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace Shared.Models;

public class Project: BaseBsonModel
{
    [BsonElement("Name")] 
    public string Name { get; set; } = "";

    [BsonElement("SprintIds")] 
    public List<MongoDBRef> SprintIds { get; set; } = new();
}