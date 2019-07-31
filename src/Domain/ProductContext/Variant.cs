using System.Collections.Generic;
using System.Linq;
using Domain.ProductContext.Events;

namespace Domain.ProductContext
{
    public class Variant : Entity
    {
        private readonly ICollection<ImageRef> _images = new HashSet<ImageRef>();

        internal Variant(string barcode, AttributeRef varianterAttribute)
        {
            Barcode = barcode;
            VarianterAttribute = varianterAttribute;
            
            Register<ImageAddedToProduct>(Apply);
        }

        private void Apply(ImageAddedToProduct @event)
        {
            _images.Add(@event.Image);
        }

        public IReadOnlyCollection<ImageRef> Images => _images.ToList();
        
        public string Barcode { get; }

        public AttributeRef VarianterAttribute { get; }
        
        public bool HasSameTypeVarianterAttribute(AttributeRef attribute)
        {
            return VarianterAttribute.AttributeId == attribute.AttributeId;
        }
    }
}