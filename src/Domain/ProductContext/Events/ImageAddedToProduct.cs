using System;

namespace Domain.ProductContext.Events
{
    public class ImageAddedToProduct :BaseProductEvent
    {
        public ImageAddedToProduct(Guid productId, AttributeRef varianterAttr, ImageRef image)
        {
            ProductId = productId;
            VarianterAttr = varianterAttr;
            Image = image;
        }

        public override Guid ProductId { get; }
        public AttributeRef VarianterAttr { get; }
        public ImageRef Image { get; }
    }
}