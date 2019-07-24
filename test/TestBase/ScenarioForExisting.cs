using System;
using System.Linq;
using Domain;
using FluentAssertions;
using KellermanSoftware.CompareNetObjects;

namespace TestBase
{
    public class ScenarioForExisting<TAggregateRoot> where TAggregateRoot : IAggregateRoot
    {
        private TAggregateRoot _aggregateRoot;
        private Action<TAggregateRoot>[] _whens;

        public ScenarioForExisting<TAggregateRoot> Given(Func<TAggregateRoot> aggregateRoot)
        {
            _aggregateRoot = aggregateRoot();
            _aggregateRoot.MarkChangesAsCommitted();
            return this;
        }

        public ScenarioForExisting<TAggregateRoot> When(params Action<TAggregateRoot>[] whens)
        {
            _whens = whens;
            return this;
        }

        public ScenarioForExisting<TAggregateRoot> With(Action<TAggregateRoot> act)
        {
            act(_aggregateRoot);
            return this;
        }

        public ScenarioForExisting<TAggregateRoot> Then(params object[] events)
        {
            foreach (var action in _whens)
            {
                action(_aggregateRoot);
            }

            var logic = new CompareLogic().Compare(_aggregateRoot.GetUncommittedChanges().ToArray(), events);
            logic.AreEqual.Should().Be(true, "Expected domain event(s) should be fired");
            return this;
        }

        public ScenarioForExisting<TAggregateRoot> AlsoAssert(Func<TAggregateRoot, bool> expression)
        {
            expression(_aggregateRoot).Should().Be(true, "AlsoAssert is not validated maybe event is not applied");
            return this;
        }

        public void ThenNone()
        {
            foreach (var action in _whens)
            {
                action(_aggregateRoot);
            }

            _aggregateRoot.GetUncommittedChanges().ToArray().Should()
                .BeEmpty("Aggregate should not have any events! But founded.");
        }

        public void ThenThrows<TException>(string message = null) where TException : Exception
        {
            Action act = () =>
            {
                foreach (var action in _whens)
                {
                    action(_aggregateRoot);
                }
            };

            if (message != null)
                act.Should().Throw<TException>().WithMessage(message);
            else
                act.Should().Throw<TException>();
        }
    }
}