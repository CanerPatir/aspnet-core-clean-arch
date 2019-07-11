using Microsoft.Extensions.DependencyInjection;
using TestBase;

namespace Application.Tests
{
    public abstract class CommandHandlerTestBase: TestBaseWithIoC
    {
        protected CommandHandlerTestBase()
        {
            ConfigureServices(services =>
            {
                services
                    .AddInProcessMessageBus();
            });
            Build();
        } 
    }
}