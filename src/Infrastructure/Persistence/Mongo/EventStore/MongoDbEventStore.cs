using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Messaging.Mediator;
using MongoDB.Driver;

namespace Infrastructure.Persistence.Mongo.EventStore
{
    public class MongoDbEventStore : IEventStore
    {
        private readonly IMediator _publisher;
        private readonly IMongoCollection<EventDescriptor> _collection;


        public MongoDbEventStore(IMediator publisher, IMongoDbContext _mongoDbContext)
        {
            _publisher = publisher;
            _collection = _mongoDbContext.GetCollection<EventDescriptor>("events");
        }

        private class EventDescriptor
        {
            public EventDescriptor(Guid id, object eventData, int version)
            {
                EventData = eventData;
                Version = version;
                AggregateRootId = id;
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            }

            public long Timestamp { get; }
            
            public object EventData { get; }
            public int Version { get; }

            public Guid AggregateRootId { get; }
        }

        public async Task SaveEvents(Guid aggregateId, IEnumerable<object> events, int expectedVersion)
        {
            var last = _collection
                .AsQueryable()
                .Where(e => e.AggregateRootId == aggregateId)
                .OrderBy(e => e.Version)
                .ThenBy(e => e.Timestamp).LastOrDefault();
            
            if (last != null && last.Version != expectedVersion && expectedVersion != -1)
            {
                throw new ConcurrencyException();
            }

            var i = expectedVersion;
            var eventDescriptors = events.ToList().Select(@event =>
            {
                i++;
                return new EventDescriptor(aggregateId, @event, i);
            });

            await _collection.InsertManyAsync(eventDescriptors, new InsertManyOptions
            {
                IsOrdered = true,
                BypassDocumentValidation = false
            });

            await Task.WhenAll(events.Select(@event => _publisher.PublishAsync(@event)));
        }

        public Task<List<object>> GetEventsForAggregate(Guid aggregateId)
        {
            return Task.FromResult(_collection
                .AsQueryable()
                .Where(e => e.AggregateRootId == aggregateId)
                .OrderBy(e => e.Version)
                .ThenBy(e => e.Timestamp).ToList().Select(descriptor => descriptor.EventData).ToList());

        }
    }
}