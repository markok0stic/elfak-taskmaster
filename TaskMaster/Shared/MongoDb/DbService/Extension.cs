using Microsoft.Extensions.DependencyInjection;
using Shared.Models;
using Shared.Models.Bson;

namespace Shared.MongoDb.DbService;

public static class Extension
{
    public static IServiceCollection AddMongoDbDataContext<T>(this IServiceCollection services) where T : BaseBsonModel
    {
        return services.AddSingleton<IMongoDbDataContext<T>,MongoDbDataContext<T>>();
    }
}