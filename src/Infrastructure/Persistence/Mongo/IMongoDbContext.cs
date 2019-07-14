using MongoDB.Driver;

namespace Infrastructure.Persistence.Mongo
{
    public interface IMongoDbContext
    {
        IMongoCollection<T> GetCollection<T>(string collectionName);
    }
}