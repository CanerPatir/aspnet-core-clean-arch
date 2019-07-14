using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Infrastructure.Persistence.Mongo
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMongoDbPersistence(this IServiceCollection services,
            IConfiguration configuration)
        {
            return services
                .Configure<MongoSettings>(configuration.GetSection("MongoDb"))
                .AddScoped(c => MongoDbProvider.Provide(c.GetRequiredService<IOptions<MongoSettings>>().Value))
                .AddScoped<IMongoDbContext, MongoDbContext>();
        }
    }
}