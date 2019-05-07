using System;

namespace Domain.ProductContext.Events
{
    public class VariantCreated: BaseProductEvent
    {
        public VariantCreated(Guid productId, string barcode, AttributeRef slicerAttribute, AttributeRef varianterAttribute)
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