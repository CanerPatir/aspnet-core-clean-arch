using Infrastructure.Persistence.InMemory;
using Infrastructure.Persistence.Mongo;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            return services
                .AddInMemoryEventStorePersistence()
//                .AddMongoDbPersistence(configuration)
//                .AddMongoDbEventStorePersistence(configuration)
                .AddInProcessMessageBus();
        }
    }
}