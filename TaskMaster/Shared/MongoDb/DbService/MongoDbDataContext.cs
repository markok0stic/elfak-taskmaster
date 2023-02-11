using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Shared.Models.Bson;

namespace Shared.MongoDb.DbService;

public interface IMongoDbDataContext<T> where T: BaseBsonModel
{
    Task<IEnumerable<T>?> GetAllAsync();
    Task<IEnumerable<T>?> GetManyAsync(FilterDefinition<T> filter);
    Task<T?> GetByIdAsync(ObjectId id);
    
    Task<bool> AddManyAsync(IEnumerable<T> entities);
    Task<T> AddAsync(T entity);
    
    Task<bool> UpdateManyAsync(FilterDefinition<T> filter, UpdateDefinition<T> update);
    Task<bool> UpdateAsync(T entity);
    
    Task<bool> DeleteManyAsync(FilterDefinition<T> filter);
    Task<bool> DeleteAsync(ObjectId id);
}

public class MongoDbDataContext<T>: IMongoDbDataContext<T> where T: BaseBsonModel
{
    private readonly IMongoDatabase _database;
    private readonly IMongoCollection<T> _collection;
    private readonly ILogger<MongoDbDataContext<T>> _logger;
    public MongoDbDataContext(IOptions<MongoDbOptions> options, ILogger<MongoDbDataContext<T>> logger)
    {
        var client = new MongoClient(options.Value.ConnectionString);
        _database = client.GetDatabase(options.Value.DatabaseName);
        _collection = _database.GetCollection<T>(typeof(T).Name);
        _logger = logger;
    }
    
    public async Task<IEnumerable<T>?> GetAllAsync()
    {
        IEnumerable<T>? result = null;
        try
        {
            result = (await _collection.FindAsync(new BsonDocument())).ToList();
        }
        catch (Exception e)
        {
            _logger.LogError(e,"");
        }

        return result;
    }

    public async Task<IEnumerable<T>?> GetManyAsync(FilterDefinition<T> filter)
    {
        IEnumerable<T>? result = null;
        try
        {
            result = (await _collection.FindAsync(filter)).ToList();
        }
        catch (Exception e)
        {
            _logger.LogError(e,"");
        }

        return result;
    }

    public async Task<T?> GetByIdAsync(ObjectId id)
    {
        T? result = null;
        try
        {
            result = (await _collection.FindAsync(x => x.Id == id)).FirstOrDefault();
        }
        catch (Exception e)
        {
            _logger.LogError(e,"");
        }

        return result;
    }
    
    public Task<T?> FetchReference<TQ>(T entity)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> AddManyAsync(IEnumerable<T> entities)
    {
        var result = false;
        try
        {
            await _collection.InsertManyAsync(entities);
            result = true;
        }
        catch (Exception e)
        {
            _logger.LogError(e,"");
        }

        return result;
    }

    public async Task<T> AddAsync(T entity)
    {
        try
        {
            await _collection.InsertOneAsync(entity);
        } 
        catch (Exception e)
        {
            _logger.LogError(e,"");
        }
        
        return entity;
    }

    public async Task<bool> UpdateManyAsync(FilterDefinition<T> filter, UpdateDefinition<T> update)
    {
        var result = false;
        try
        { 
            await _collection.UpdateManyAsync(filter, update);
            result = true;
        }
        catch (Exception e)
        {
            _logger.LogError(e,"");
        }

        return result;
    }

    public async Task<bool> UpdateAsync(T entity)
    {
        var result = false;
        try
        {
            await _collection.ReplaceOneAsync(x => x.Id == entity.Id, entity);
            result = true;
        }
        catch (Exception e)
        {
            _logger.LogError(e,"");
        }

        return result;
    }

    public async Task<bool> DeleteManyAsync(FilterDefinition<T> filter)
    {
        var result = false;
        try
        {
            await _collection.DeleteManyAsync(filter);
            result = true;
        }
        catch (Exception e)
        {
            _logger.LogError(e,"");
        }

        return result;
    }

    public async Task<bool> DeleteAsync(ObjectId id)
    {
        var result = false;
        try
        {
            await _collection.DeleteOneAsync(x => x.Id == id);
            result = true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "");
        }

        return result;
    }
}