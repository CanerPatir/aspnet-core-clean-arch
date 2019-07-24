using System;

namespace Domain.ProductContext.Events
{
    public class VariantAddedToProduct: BaseProductEvent
    {
        public VariantAddedToProduct(Guid productId, string barcode, AttributeRef slicerAttribute, AttributeRef varianterAttribute)
        {
            ProductId = productId;
            Barcode = barcode;
            SlicerAttribute = slicerAttribute;
            VarianterAttribute = varianterAttribute;
        }

        public override Guid ProductId { get; }
        public string Barcode { get; }
        public AttributeRef SlicerAttribute { get; }
        public AttributeRef VarianterAttribute { get; }
    }
}