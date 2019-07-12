using System;
using System.Threading;
using System.Threading.Tasks;
using Application;
using Infrastructure.Messaging.Mediator;
using Microsoft.Extensions.DependencyInjection;
using TestBase;
using Xunit;

namespace Infrastructure.Tests
{
    public class MediatorTests : TestBaseWithIoC
    {
        internal static Guid Command_Should_Be_Handled_Test_Assertion = Guid.Empty;
        public MediatorTests()
        {
            ConfigureServices(services =>
                {
                    services
                        .AddInProcessMessageBus();
                }).
                Build();
        }

        [Fact]
        public async Task Command_Should_Be_Handled()
        {
           var bus = GetRequiredService<IMediator>();

           var newGuid = Guid.NewGuid();
           await bus.SendAsync(new TestCommand(newGuid));
           
           Assert.Equal(newGuid, Command_Should_Be_Handled_Test_Assertion);
        }
        
    }
    
    public class TestCommand
    {
        public TestCommand(Guid test)
        {
            Test = test;
        }

        public Guid Test { get; }    
    }

    public class TestCommandHandler : ICommandHandler<TestCommand>
    {
        public Task Handle(TestCommand message, CancellationToken cancellationToken)
        {
            MediatorTests.Command_Should_Be_Handled_Test_Assertion = message.Test;
            
            return Task.CompletedTask;
        }
    }
}