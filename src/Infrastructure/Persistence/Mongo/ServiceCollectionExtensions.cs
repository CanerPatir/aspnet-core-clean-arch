using Domain.ProductContext;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure.Persistence.Mongo
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMongoDbPersistence(this IServiceCollection services,
            IConfiguration configuration)
        {
            return services
                .Configure<MongoSettings>(configuration.GetSection("MongoDb"))
                .AddSingleton(c => MongoDbProvider.Provide(c.GetRequiredService<IOptions<MongoSettings>>().Value))
                .AddSingleton<IMongoDbContext, MongoDbContext>()
                .AddScoped<IProductRepository, MongoDbProductRepository>();
        }
    }
}