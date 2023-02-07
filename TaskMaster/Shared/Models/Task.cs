using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace Shared.Models;

public class Task: BaseBsonModel
{
    [BsonElement("Name")] 
    public string Name { get; set; } = "";

    [BsonElement("SprintIds")] 
    public List<MongoDBRef> SprintIds { get; set; } = new();

    [BsonElement("ProjectId")] 
    public MongoDBRef ProjectId { get; set; } = new("", "");

    [BsonElement("Description")] 
    public string Description { get; set; } = "";

    [BsonElement("Estimate")] 
    public decimal Estimate { get; set; } = 0;
    
    [BsonElement("Discussion")] 
    public List<Comment> Discussion { get; set; } = new();
}