using System.Collections;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Shared.Models.Bson;

namespace Shared.Models;

public class Sprint: BaseBsonModel
{
    [BsonElement("Name")] 
    public string Name { get; set; } = "";

    [BsonElement("ProjectId")] 
    public DocumentReference ProjectId { get; set; } = new();

    [BsonElement("TaskIds")]
    public List<DocumentReference> TaskIds { get; set; } = new();
    
    [BsonIgnore] 
    public Project? Project { get; set; }

    [BsonIgnore]
    public IEnumerable<Task>? Tasks { get; set; }
}



