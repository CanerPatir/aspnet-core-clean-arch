namespace Domain.Abstraction
{
    public interface IRepository<T, TKey> where T : AggregateRoot<TKey>
    {
        void Save(T aggregate, int expectedVersion);
        T Load(TKey id);
    }
}