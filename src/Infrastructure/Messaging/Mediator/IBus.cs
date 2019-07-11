using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Messaging.Mediator
{
    public interface IBus
    {
        Task SendAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default);
        Task<TResult> SendAsync<TMessage, TResult>(TMessage message, CancellationToken cancellationToken = default);
    }
}