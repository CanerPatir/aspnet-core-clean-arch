using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.ProductContext;

namespace Infrastructure.Persistence
{
    internal class EventStoreProductRepository : IProductRepository
    {
        private readonly IEventStore _eventStore;
        
        public EventStoreProductRepository(IEventStore eventStore) => _eventStore = eventStore;

        public async Task Save(Product aggregate, CancellationToken cancellationToken)
        {
           await _eventStore.SaveEvents(aggregate.Id, aggregate.GetUncommittedChanges(), aggregate.Version);
           aggregate.MarkChangesAsCommitted();
        }

        public async Task<Product> Load(Guid id, CancellationToken cancellationToken)
        {
            var aggregate = new Product();//lots of ways to do this
            var e = await _eventStore.GetEventsForAggregate(id);
            aggregate.LoadsFromHistory(e);
            return aggregate;
        }
    }
}