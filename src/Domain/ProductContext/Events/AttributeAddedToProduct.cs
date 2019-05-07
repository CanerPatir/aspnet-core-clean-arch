using System;

namespace Domain.ProductContext.Events
{
    public class AttributeAddedToProduct : BaseProductEvent
    {
        public AttributeAddedToProduct(Guid productId, AttributeRef attribute)
        {
            ProductId = productId;
            Attribute = attribute;
        }

        public override Guid ProductId { get; }
        public AttributeRef Attribute { get; }
    }
}