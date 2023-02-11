using System.Collections;
using MongoDB.Bson.Serialization.Attributes;
using Shared.Models.Bson;

namespace Shared.Models;

public class Project: BaseBsonModel
{
    [BsonElement("Name")] 
    public string Name { get; set; } = "";

    [BsonElement("SprintIds")] 
    public List<DocumentReference> SprintIds { get; set; } = new();

    [BsonIgnore] 
    public IEnumerable<Sprint>? Sprints { get; set; }
}