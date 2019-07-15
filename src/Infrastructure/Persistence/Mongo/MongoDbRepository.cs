using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using Domain.Abstraction;
using Infrastructure.Messaging.Mediator;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Infrastructure.Persistence.Mongo
{
    
    public abstract class MongoDbRepository<TAggregate, TKey> : IRepository<TAggregate, TKey>
        where TAggregate : AggregateRoot<TKey>
    {
        protected abstract string CollectionName { get; }

        private readonly IMediator _mediator;
        private readonly IMongoCollection<TAggregate> _collection;
        
        protected MongoDbRepository(IMediator mediator,IMongoDbContext context )
        {
            _mediator = mediator;
            _collection = context.GetCollection<TAggregate>(CollectionName);
        }
        
        public virtual async Task Save(TAggregate aggregate, CancellationToken cancellationToken)
        {
            var existing = await _collection.AsQueryable().Where(p => p.Id.Equals(aggregate.Id)).FirstOrDefaultAsync(cancellationToken);
            if (existing != null)
            {
                if (existing.Version > aggregate.Version)
                {
                    throw new ConcurrencyException();
                }

                await _collection.ReplaceOneAsync(c => c.Id.Equals(aggregate.Id), aggregate, new UpdateOptions()
                {
                    IsUpsert = true
                }, cancellationToken);
            }
            else
            {
                await _collection.InsertOneAsync(aggregate, new InsertOneOptions
                {
                    BypassDocumentValidation = false
                }, cancellationToken);
            }
            
            // todo: find a way to prevent inconsistency between state and domain events
            await DispatchDomainEventsAsync(aggregate, cancellationToken);
            
        }
        protected async Task DispatchDomainEventsAsync(TAggregate aggregateRoot, CancellationToken cancellationToken)
        {
            var domainEvents = aggregateRoot.GetUncommittedChanges();
            aggregateRoot.MarkChangesAsCommitted();
            
            await Task.WhenAll(domainEvents
                .Select(async domainEvent => await _mediator.PublishAsync(domainEvent, cancellationToken)));
        }
        
        public virtual Task<TAggregate> Load(TKey id, CancellationToken cancellationToken)
        {
            return _collection.AsQueryable().Where(p => p.Id.Equals(id)).FirstOrDefaultAsync(cancellationToken);
        }
    }
}