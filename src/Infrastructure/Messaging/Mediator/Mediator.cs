using System;
using System.Threading;
using System.Threading.Tasks;
using Application;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Messaging.Mediator
{
    public class Mediator : IMediator
    {
        private readonly IServiceProvider _serviceProvider;

        public Mediator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task SendAsync<TMessage>(TMessage message, CancellationToken cancellationToken)
        {
            var handler = _serviceProvider.GetRequiredService<ICommandHandler<TMessage>>();
            if (handler == null) throw new NullReferenceException($"handler of {nameof(TMessage)}");

            return handler.Handle(message, cancellationToken);
        }

        public Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        {
            var handler = _serviceProvider.GetRequiredService<IEventHandler<TEvent>>();
            if (handler == null) throw new NullReferenceException($"handler of {nameof(TEvent)}");

            return handler.Handle(@event, cancellationToken);        }

        public Task<TResult> SendAsync<TMessage, TResult>(TMessage message, CancellationToken cancellationToken)
        {
            var handler = _serviceProvider.GetRequiredService<IQueryHandler<TMessage, TResult>>();
            if (handler == null)
                throw new NullReferenceException($"handler of {nameof(TMessage)} which returning {nameof(TResult)}");

            return handler.Handle(message, cancellationToken);
        }
    }
}