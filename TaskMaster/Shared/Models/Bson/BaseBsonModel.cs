using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Shared.Models.Bson;

public class BaseBsonModel
{
    [BsonId]
    public ObjectId Id { get; set; }

    [BsonIgnore] 
    [JsonPropertyName("_id")]
    public string StringId => GetStringRepresentationOfObjectId();

    private string GetStringRepresentationOfObjectId()
    {
        return Id.ToString();
    }
}