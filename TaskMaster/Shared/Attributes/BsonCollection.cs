using MongoDB.Bson.Serialization.Attributes;

namespace Shared.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class BsonCollection : BsonElementAttribute
{
    public Type Type { get; set; }
    public string Name { get; set; }

    public BsonCollection(Type type)
    {
        Type = type;
        Name = type.Name;
    }
}