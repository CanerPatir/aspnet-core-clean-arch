using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Domain
{
    public interface IAggregateRoot
    {
        int Version { get; }
        bool HasChanges();
        IEnumerable<object> GetUncommittedChanges();
        void MarkChangesAsCommitted();
        void LoadsFromHistory(IEnumerable<object> history);
    }

    public abstract class AggregateRoot<TKey> : IAggregateRoot
    {
        private readonly List<object> _changes = new List<object>();
        private readonly InstanceEventRouter _router;

        public AggregateRoot()
        {
            _router = new InstanceEventRouter();
        }

        public virtual TKey Id { get; protected set; }

        public virtual int Version { get; protected set; }

        public bool HasChanges() => _changes.Any();

        public IEnumerable<object> GetUncommittedChanges()
        {
            return _changes;
        }

        public void MarkChangesAsCommitted()
        {
            _changes.Clear();
        }

        public void LoadsFromHistory(IEnumerable<object> history)
        {
            foreach (var e in history) ApplyChange(e, false);
        }

        protected void Register<TEvent>(Action<TEvent> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            _router.ConfigureRoute(handler);
        }

        protected void ApplyChange(object @event)
        {
            ApplyChange(@event, true);
        }

        protected void ApplyChange(object @event, bool isNew)
        {
            if (@event == null) throw new ArgumentNullException(nameof(@event));

            _router.Route(@event);
            if (isNew)
            {
                _changes.Add(@event);
                Version++;
            }
        }

        #region overrides

        /// <inheritdoc />
        /// <summary>
        ///     Checks if this entity is transient (it has not an Id).
        /// </summary>
        /// <returns>True, if this entity is transient</returns>
        public virtual bool IsTransient()
        {
            if (EqualityComparer<TKey>.Default.Equals(Id, default))
            {
                return true;
            }

            //Workaround for EF Core since it sets int/long to min value when attaching to dbcontext
            if (typeof(TKey) == typeof(int))
            {
                return Convert.ToInt32(Id) <= 0;
            }

            if (typeof(TKey) == typeof(long))
            {
                return Convert.ToInt64(Id) <= 0;
            }

            return false;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is AggregateRoot<TKey>))
            {
                return false;
            }

            //Same instances must be considered as equal
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            //Transient objects are not considered as equal
            var other = (AggregateRoot<TKey>) obj;
            if (IsTransient() && other.IsTransient())
            {
                return false;
            }

            //Must have a IS-A relation of types or must be same type
            var typeOfThis = GetType();
            var typeOfOther = other.GetType();
            if (!typeOfThis.GetTypeInfo().IsAssignableFrom(typeOfOther) &&
                !typeOfOther.GetTypeInfo().IsAssignableFrom(typeOfThis))
            {
                return false;
            }

            return Id.Equals(other.Id);
        }

        public override int GetHashCode() => Id.GetHashCode();

        public static bool operator ==(AggregateRoot<TKey> left, AggregateRoot<TKey> right)
        {
            if (Equals(left, null))
            {
                return Equals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(AggregateRoot<TKey> left, AggregateRoot<TKey> right) =>
            !(left == right);

        #endregion

        #region rule helpers

        protected void Should(Func<bool> predicate)
        {
            Should(predicate());
        }

        protected void Should(Func<bool> predicate, string otherwiseMessage)
        {
            Should(predicate(), otherwiseMessage);
        }

        protected void Should(bool clause)
        {
            if (clause) return;
            throw new BusinessException();
        }

        protected void Should(bool clause, string otherwiseMessage)
        {
            if (clause) return;
            throw new BusinessException(otherwiseMessage);
        }

        #endregion
    }
}