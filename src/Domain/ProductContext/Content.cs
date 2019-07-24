using System.Collections.Generic;
using System.Linq;
using Domain.ProductContext.Events;

namespace Domain.ProductContext
{
    /// <summary>
    /// Represents group of product variant and can be projection of product detail page.  
    /// </summary>
    public class Content : Entity
    {
        private ICollection<Variant> _variants = new HashSet<Variant>();

        internal Content(string title, string description, AttributeRef slicerAttribute)
        {
            Title = title;
            Description = description;
            SlicerAttribute = slicerAttribute;
            
            Register<VariantAddedToProduct>(Apply);
        }

        private void Apply(VariantAddedToProduct @event)
        {
            _variants.Add(new Variant(@event.Barcode, @event.VarianterAttribute));
        }

        public IReadOnlyCollection<Variant> Variants => _variants.ToList();
        
        public string Title { get; private set; }

        public string Description { get; private set; }

        public AttributeRef SlicerAttribute { get; private set; }

        public bool HasSameTypeSlicerAttribute(AttributeRef attribute)
        {
            return SlicerAttribute.AttributeId == attribute.AttributeId;
        }

    }
}