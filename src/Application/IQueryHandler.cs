using System.Threading.Tasks;

namespace Application
{
    public interface IQueryHandler<in TMessage, TResult>
    {
        Task<TResult> HandleAsync(TMessage message);
    }
}