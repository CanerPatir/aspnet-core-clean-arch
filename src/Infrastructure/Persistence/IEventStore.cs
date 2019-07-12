using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public interface IEventStore
    {
        Task SaveEvents(Guid aggregateId, IEnumerable<object> events, int expectedVersion);
        Task<List<object>> GetEventsForAggregate(Guid aggregateId);
    }
}