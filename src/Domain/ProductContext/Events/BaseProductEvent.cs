using System;
using Domain.Abstraction;

namespace Domain.ProductContext.Events
{
    public abstract class BaseProductEvent : BaseDomainEvent
    {
        public abstract Guid ProductId { get; }
    }
}