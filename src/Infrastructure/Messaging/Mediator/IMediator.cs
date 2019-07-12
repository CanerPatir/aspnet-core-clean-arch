using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Messaging.Mediator
{
    public interface IMediator
    {
        Task SendAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default);
        Task PublishAsync<TEvent>(TEvent message, CancellationToken cancellationToken = default);
        Task<TResult> SendAsync<TMessage, TResult>(TMessage @event, CancellationToken cancellationToken = default);
    }
}