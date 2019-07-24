using FluentAssertions;
using KellermanSoftware.CompareNetObjects;
using System;
using System.Linq;
using Domain;

namespace TestBase
{
    public class ScenarioFor<TAggregateRoot> where TAggregateRoot : IAggregateRoot
    {
        private TAggregateRoot _aggregateRoot;
        private Action<TAggregateRoot>[] _whens;

        public ScenarioFor(Func<TAggregateRoot> constructor) => _aggregateRoot = constructor();

        public ScenarioFor<TAggregateRoot> When(params Action<TAggregateRoot>[] whens)
        {
            _whens = whens;
            return this;
        }

        public ScenarioFor<TAggregateRoot> With(Action<TAggregateRoot> act)
        {
            act(_aggregateRoot);
            return this;
        }

        public void Then(params object[] events)
        {
            var logic = new CompareLogic().Compare(_aggregateRoot.GetUncommittedChanges().ToArray(), events);
            logic.AreEqual.Should().Be(true, "Expected domain event(s) should be fired");
        }

        public void ThenThrows<TException>(string message = "") where TException : Exception
        {
            Action act = () =>
            {
                foreach (var action in _whens)
                {
                    action(_aggregateRoot);
                }
            };

            act.Should().Throw<TException>(message);
        }
    }
}