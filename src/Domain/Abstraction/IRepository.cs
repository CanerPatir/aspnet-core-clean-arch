using System.Threading.Tasks;

namespace Domain.Abstraction
{
    /// <summary>
    /// Provides an abstraction for object sotrage and
    /// </summary>
    /// <typeparam name="T">Type of aggregate root</typeparam>
    /// <typeparam name="TKey">Type of aggregate root's key</typeparam>
    public interface IRepository<T, TKey> where T : AggregateRoot<TKey>
    {
        Task Save(T aggregate);
        Task<T> Load(TKey id);
    }
}