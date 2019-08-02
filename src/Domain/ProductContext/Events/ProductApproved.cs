using System;

namespace Domain.ProductContext.Events
{
    public class ProductApproved : BaseProductEvent
    {
        public ProductApproved(Guid productId)
        {
            ProductId = productId;
        }

        public override Guid ProductId { get; }
    }
}