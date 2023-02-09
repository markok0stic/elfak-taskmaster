using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Shared.Attributes;

namespace Shared.Models;

public class Task: BaseBsonModel
{
    [BsonElement("Name")] 
    public string Name { get; set; } = "";

    [BsonElement("SprintIds")] 
    [BsonCollection(typeof(Sprint))]
    public List<MongoDBRef> SprintIds { get; set; } = new();

    [BsonElement("ProjectId")] 
    [BsonCollection(typeof(Project))]
    public MongoDBRef ProjectId { get; set; } = new("", "");

    [BsonElement("Description")] 
    public string Description { get; set; } = "";

    [BsonElement("Estimate")] 
    public decimal Estimate { get; set; } = 0;
    
    [BsonElement("Discussion")] 
    public List<Comment> Discussion { get; set; } = new();
    
    [BsonIgnore]
    public List<Sprint> Sprints { get; set; } = new();
    
    [BsonIgnore]
    public Project Project { get; set; } = new();
}