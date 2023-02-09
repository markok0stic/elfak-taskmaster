using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Shared.Attributes;

namespace Shared.Models;

public class Sprint: BaseBsonModel
{
    [BsonElement("Name")] 
    public string Name { get; set; } = "";

    [BsonElement("ProjectId")] 
    [BsonCollection(typeof(Project))]
    public MongoDBRef ProjectId { get; set; } = new("","");

    [BsonElement("TaskIds")] 
    public List<MongoDBRef> TaskIds { get; set; } = new();
    
    [BsonIgnore] 
    public Project Project { get; set; } = new();

    [BsonIgnore] 
    public List<Task> Tasks { get; set; } = new();
}



