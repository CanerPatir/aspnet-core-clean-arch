using Domain.ProductContext;
using Infrastructure.Persistence.InMemory.EventStore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence.InMemory
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInMemoryEventStorePersistence(this IServiceCollection services)
        {
            return services
                .AddSingleton<IEventStore, InMemoryEventStore>()
                .AddScoped<IProductRepository, EventStoreProductRepository>();
        }
    }
}