using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Shared.Models;

namespace Shared.MongoDb.DbService;

public interface IMongoDbDataContext<T> where T: BaseBsonModel
{
    Task<IEnumerable<T>?> GetAllAsync();
    Task<IEnumerable<T>?> GetManyAsync(FilterDefinition<T> filter);
    Task<T?> GetByIdAsync(ObjectId id);
    
    Task<bool> AddManyAsync(IEnumerable<T> entities);
    Task<T> AddAsync(T entity);
    
    Task<bool> UpdateManyAsync(FilterDefinition<T> filter, UpdateDefinition<T> update);
    Task<bool> UpdateAsync(ObjectId id, T entity);
    
    Task<bool> DeleteManyAsync(FilterDefinition<T> filter);
    Task<bool> DeleteAsync(ObjectId id);
}

public class MongoDbDataContext<T>: IMongoDbDataContext<T> where T: BaseBsonModel
{
    private readonly IMongoCollection<T> _collection;
    private readonly ILogger<MongoDbDataContext<T>> _logger;

    public MongoDbDataContext(IOptions<MongoDbOptions> options, ILogger<MongoDbDataContext<T>> logger)
    {
        var client = new MongoClient(options.Value.ConnectionString);
        var database = client.GetDatabase(options.Value.DatabaseName);
        _collection = database.GetCollection<T>(nameof(T));
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

    public async Task<bool> UpdateAsync(ObjectId id, T entity)
    {
        var result = false;
        entity.Id = id;
        try
        {
            await _collection.ReplaceOneAsync(x => x.Id == id, entity);
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