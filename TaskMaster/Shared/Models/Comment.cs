using MongoDB.Bson.Serialization.Attributes;

namespace Shared.Models;
public class Comment
{
    [BsonElement("Author")]
    public string Author { get; set; } = "";

    [BsonElement("Message")] 
    public string Message { get; set; } = "";
}