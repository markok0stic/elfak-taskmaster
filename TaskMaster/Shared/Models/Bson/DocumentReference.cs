using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Shared.Models.Bson;
/// <summary>
/// I had a problem using MongoDbRef so i made an DbRef type so i know that the property is ref to another doc.
/// Here is the error: System.InvalidCastException: Unable to cast object of type 'MongoDB.Bson.BsonObjectId' to type 'MongoDB.Bson.BsonBoolean'.
/// But i dont have any casts at all like that, the object fetches well and its references but when it comes in controller to return Ok(object),
/// it returns it and then i see mentioned error really weird.
/// </summary>
public class DocumentReference
{
    public ObjectId Id { get; set; }
    public string CollectionName { get; set; }
    
    [BsonIgnore] 
    [JsonPropertyName("_id")]
    public string StringId => GetStringRepresentationOfObjectId();
 
    private string GetStringRepresentationOfObjectId()
    {
        return Id.ToString();
    }
    
    public DocumentReference()
    {
        CollectionName = "";
    }
    public DocumentReference(string collectionName, ObjectId id)
    {
        Id = id;
        CollectionName = collectionName;
    }
}