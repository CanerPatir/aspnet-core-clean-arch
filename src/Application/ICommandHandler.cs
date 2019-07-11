using System.Threading.Tasks;

namespace Application
{
    public interface ICommandHandler<in TMessage>
    {
        Task HandleAsync(TMessage message);
    }
}