using System;

namespace Domain.ProductContext.Events
{
    public class ProductCreated : BaseProductEvent
    {
        public ProductCreated(Guid productId, string productCode, int brandId, int categoryId)
        {
            ProductId = productId;
            BrandId = brandId;
            ProductCode = productCode;
            CategoryId = categoryId;
        }

        public override Guid ProductId { get; }
        public int BrandId { get; }
        public string ProductCode { get; }
        public int CategoryId { get; }
    }
}