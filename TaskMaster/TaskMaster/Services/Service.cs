using Shared.Models;
using Shared.MongoDb.DbService;

namespace TaskMaster.Services;

public class Service
{
    private readonly IMongoDbDataContext<Class> _dataContext;
    public Service(IMongoDbDataContext<Class> dataContext)
    {
        _dataContext = dataContext;
    }
}