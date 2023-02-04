using Microsoft.Extensions.DependencyInjection;

namespace Shared.MongoDb.DbService;

public static class Extension
{
    public static IServiceCollection AddMongoDbDataContext<T>(this IServiceCollection services) where T : class
    {
        return services.AddSingleton<IMongoDbDataContext<T>,MongoDbDataContext<T>>();
    }
}