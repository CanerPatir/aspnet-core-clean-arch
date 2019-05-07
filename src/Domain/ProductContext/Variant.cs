namespace Domain.ProductContext
{
    public class Variant : Entity
    {
        internal Variant(string barcode, AttributeRef varianterAttribute)
        {
            Barcode = barcode;
            VarianterAttribute = varianterAttribute;
        }

        public string Barcode { get; }

        public AttributeRef VarianterAttribute { get; }
        
        public bool HasSameTypeVarianterAttribute(AttributeRef attribute)
        {
            return VarianterAttribute.AttributeId == attribute.AttributeId;
        }
    }
}