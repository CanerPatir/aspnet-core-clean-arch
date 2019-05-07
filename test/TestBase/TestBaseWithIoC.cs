using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FakeItEasy;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TestBase
{
    public abstract class TestBaseWithIoC : IDisposable
    {
        private IServiceProvider _localResolver;
        private readonly IServiceCollection _services;

        protected TestBaseWithIoC() => _services = new ServiceCollection();

        protected IConfiguration Configuration => InMemoryConfiguration != null ? 
                                                    new ConfigurationBuilder().AddInMemoryCollection(InMemoryConfiguration).Build():
                                                    new ConfigurationBuilder().AddJsonFile(ConfigJsonFilename).Build();

        protected virtual IDictionary<string, string> InMemoryConfiguration => null;

        protected virtual string ConfigJsonFilename => "appsettings.test.json";

        public virtual void Dispose()
        {
        }

        protected TestBaseWithIoC Building(Action<IServiceCollection> builder)
        {
            builder(_services);
            return this;
        }

        public void Ok()
        {
            PreBuild();
            _localResolver = _services.BuildServiceProvider();
            PostBuild();
        }

        protected virtual void PreBuild()
        {
        }

        protected virtual void PostBuild()
        {
        }

        protected T The<T>() => _localResolver.GetRequiredService<T>();

        protected virtual T Fake<T>(bool strict = false) where T : class
        {
            return A.Fake<T>(cfg =>
            {
                if (strict)
                {
                    cfg.Strict();
                }
            });
        }

        protected virtual T FakeAndRegister<T>(bool strict = false)
            where T : class
        {
            T a = Fake<T>();

            _services.AddSingleton(c => a);

            return a;
        }

        protected virtual T FakeAndRegisterWithType<T>(bool strict = false)
            where T : class
        {
            T a = Fake<T>();

            _services.AddSingleton(typeof(T), c => a);

            return a;
        }

        protected virtual void Verify(Expression<Action> callSpecification, int numberOfTimes = 1)
        {
            A.CallTo(callSpecification).MustHaveHappened(numberOfTimes, Times.Exactly);
        }

        protected virtual void Stub<T>(Expression<Func<T>> callSpecification, T stubbingValue)
        {
            A.CallTo(callSpecification).Returns(stubbingValue);
        }

        protected virtual T Ignore<T>() => A<T>.Ignored;

        protected T Random<T>() => TestBase.Random<T>._;
    }
}