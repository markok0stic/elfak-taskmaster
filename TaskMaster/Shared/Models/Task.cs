using System.Collections;
using MongoDB.Bson.Serialization.Attributes;
using Shared.Models.Bson;

namespace Shared.Models;

public class Task: BaseBsonModel
{
    [BsonElement("Name")] 
    public string Name { get; set; } = "";

    [BsonElement("SprintIds")] 
    public List<DocumentReference> SprintIds { get; set; } = new();

    [BsonElement("ProjectId")] 
    public DocumentReference ProjectId { get; set; } = new();

    [BsonElement("Description")] 
    public string Description { get; set; } = "";

    [BsonElement("Estimate")] 
    public decimal Estimate { get; set; } = 0;
    
    [BsonElement("Discussion")] 
    public List<Comment> Discussion { get; set; } = new();
    
    [BsonIgnore]
    public IEnumerable<Sprint>? Sprints { get; set; }
    
    [BsonIgnore]
    public Project? Project { get; set; }
}