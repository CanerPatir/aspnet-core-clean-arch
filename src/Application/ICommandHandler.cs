using System.Threading;
using System.Threading.Tasks;

namespace Application
{
    public interface ICommandHandler<in TMessage>
    {
        Task Handle(TMessage message, CancellationToken cancellationToken);
    }
}