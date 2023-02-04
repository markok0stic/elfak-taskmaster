using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.MongoDb;

public static class Extension
{
    public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection(nameof(MongoDbOptions));
        return services.Configure<MongoDbOptions>(section);
    }
}