using System;

namespace Domain
{
    public abstract class Entity 
    {
        private readonly InstanceEventRouter _router;

        protected Entity() => _router = new InstanceEventRouter();

        protected void Register<TEvent>(Action<TEvent> handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            _router.ConfigureRoute(handler);
        }

        public virtual void Route(object @event)
        {
            if (@event == null)
            {
                throw new ArgumentNullException(nameof(@event));
            }

            _router.Route(@event);
        }
    }
}