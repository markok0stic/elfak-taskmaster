using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Shared.MongoDb.DbService;

public interface IMongoDbDataContext<T> where T: class
{
    
}

public class MongoDbDataContext<T>: IMongoDbDataContext<T> where T: class
{
    private readonly IMongoClient _client;
    private readonly IMongoDatabase _database;
    private readonly IMongoCollection<T> _collection;

    public MongoDbDataContext(IOptions<MongoDbOptions> options)
    {
        _client = new MongoClient(options.Value.ConnectionString);
        _database = _client.GetDatabase(options.Value.DatabaseName);
        _collection = _database.GetCollection<T>(options.Value.CollectionName);
    }
    
    
}