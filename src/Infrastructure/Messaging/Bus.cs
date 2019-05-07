using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Messaging
{
    public interface IBus
    {
        Task SendAsync<TMessage>(TMessage message, CancellationToken cancellationToken);
        Task<TResult> SendAsync<TMessage, TResult>(TMessage message, CancellationToken cancellationToken);
    }

    public class Bus : IBus
    {
        private readonly IServiceProvider _serviceProvider;

        public Bus(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task SendAsync<TMessage>(TMessage message, CancellationToken cancellationToken)
        {
            var handler = _serviceProvider.GetRequiredService<IHandle<TMessage>>();
            if (handler == null) throw new NullReferenceException($"handler of {nameof(TMessage)}");

            return handler.HandleAsync(message);
        }

        public Task<TResult> SendAsync<TMessage, TResult>(TMessage message, CancellationToken cancellationToken)
        {
            var handler = _serviceProvider.GetRequiredService<IHandle<TMessage, TResult>>();
            if (handler == null)
                throw new NullReferenceException($"handler of {nameof(TMessage)} which returning {nameof(TResult)}");

            return handler.HandleAsync(message);
        }
    }
}