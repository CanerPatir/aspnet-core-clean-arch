using System;
using Domain.ProductContext;
using Infrastructure.Messaging.Mediator;

namespace Infrastructure.Persistence.Mongo
{
    public class MongoDbProductRepository : MongoDbRepository<Product, Guid>, IProductRepository
    {
        public MongoDbProductRepository(IMongoDbContext context, IMediator mediator) : base(mediator, context)
        {
        }

        protected override string CollectionName => "products";
    }
}