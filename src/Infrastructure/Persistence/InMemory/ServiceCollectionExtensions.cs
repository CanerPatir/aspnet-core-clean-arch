using Domain.ProductContext;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence.InMemory
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInMemoryPersistence(this IServiceCollection services)
        {
            return services
                .AddSingleton<IEventStore, InMemoryEventStore>()
                .AddScoped<IProductRepository, InMemoryProductRepository>();
        }
    }
}