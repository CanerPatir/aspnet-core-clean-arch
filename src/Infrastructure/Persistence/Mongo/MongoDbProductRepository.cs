using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.ProductContext;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Infrastructure.Persistence.Mongo
{
    public class MongoDbProductRepository : IProductRepository
    {
        private readonly IMongoCollection<Product> _collection;

        public MongoDbProductRepository(IMongoDbContext context)
        {
            _collection = context.GetCollection<Product>("product");
        }        
        
        public Task Save(Product aggregate, CancellationToken cancellationToken)
        {  
           return _collection.InsertOneAsync(aggregate, new InsertOneOptions()
           {
               BypassDocumentValidation = false
           }, cancellationToken);
        }

        public Task<Product> Load(Guid id, CancellationToken cancellationToken)
        {
            return _collection.AsQueryable().Where(p => p.Id == id).FirstOrDefaultAsync(cancellationToken);
        }
    }
}