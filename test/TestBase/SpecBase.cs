using System;
using Domain;

namespace TestBase
{
    public abstract class SpecBase
    {
        protected T Random<T>() => TestBase.Random<T>._;

        protected ScenarioForExisting<TAggregate> ScenarioForExisting<TAggregate>() where TAggregate : IAggregateRoot
        {
            return new ScenarioForExisting<TAggregate>();
        }
        
        protected ScenarioFor<TAggregate> ScenarioFor<TAggregate>(Func<TAggregate> constructor) where TAggregate : IAggregateRoot
        {
            return new ScenarioFor<TAggregate>(constructor);
        }
    }
}
