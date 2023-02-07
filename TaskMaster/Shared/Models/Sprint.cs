using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace Shared.Models;

public class Sprint: BaseBsonModel
{
    [BsonElement("Name")] 
    public string Name { get; set; } = "";

    [BsonElement("ProjectId")] 
    public MongoDBRef ProjectId { get; set; } = new("","");

    [BsonElement("TaskIds")] 
    public List<MongoDBRef> TaskIds { get; set; } = new();
}



