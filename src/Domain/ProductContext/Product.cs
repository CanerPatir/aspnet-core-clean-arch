using System;
using System.Collections.Generic;
using System.Linq;
using Domain.ProductContext.Events;

namespace Domain.ProductContext
{
    public class Product : AggregateRoot<Guid>
    {
        private readonly ICollection<AttributeRef> _attributes = new HashSet<AttributeRef>();

        private readonly ICollection<Content> _contents = new HashSet<Content>();

        public Product()
        {
            Register<ProductCreated>(Apply);
            Register<ContentAddedToProduct>(Apply);
            Register<AttributeAddedToProduct>(Apply);
            Register<VariantAddedToProduct>(Apply);
            Register<ImageAddedToProduct>(Apply);
        }

        public static Product Create(Guid productId,
            int categoryId,
            int brandId,
            string productCode)
        {
            var product =  new Product();
            product.ApplyChange(
                new ProductCreated(productId,
                    productCode,
                    brandId, categoryId));

            return product;
        }

        public BrandRef Brand { get; private set; }

        public CategoryRef Category { get; private set; }

        public string Code { get; private set; }

        public IReadOnlyCollection<Content> Contents => _contents.ToList();

        public IReadOnlyCollection<AttributeRef> Attributes => _attributes.ToList();

        public bool IsApproved { get; private set; }

        private void Apply(ContentAddedToProduct @event) => _contents.Add(new Content(@event.Title, @event.Description, @event.SlicerAttribute));

        private void Apply(AttributeAddedToProduct @event) => _attributes.Add(@event.Attribute);

        private void Apply(VariantAddedToProduct @event) => _contents.First(c => c.SlicerAttribute == @event.SlicerAttribute).Route(@event);

        private void Apply(ProductCreated @event)
        {
            Id = @event.ProductId;
            Brand = new BrandRef(@event.BrandId, "");
            Category = new CategoryRef(@event.CategoryId, "");
            Code = @event.ProductCode;
        }
        
        private void Apply(ImageAddedToProduct @event)
        {
            var variant = _contents.SelectMany(c => c.Variants)
                .SingleOrDefault(v => v.VarianterAttribute == @event.VarianterAttr);
            
            // ReSharper disable once PossibleNullReferenceException
            variant.Route(variant);
        }
        
        public void AddContent(string title, string description, AttributeRef slicerAttribute)
        {
            if (_contents.Any())
            {
                Should(() => _contents.Any(c => c.HasSameTypeSlicerAttribute(slicerAttribute)),
                    "Given attribute type should belong to any content of product as slicer");
            }
            
            Should(() => _contents.All(c => c.SlicerAttribute != slicerAttribute),
                "Same content already exists with given attribute");

            ApplyChange(new ContentAddedToProduct(Id, title, description, slicerAttribute));
        }
        
        public void AddVariant(string barcode, AttributeRef slicerAttribute, AttributeRef varianterAttribute)
        {
            var content = _contents.SingleOrDefault(c => c.SlicerAttribute == slicerAttribute);
            Should(() => content != null, "No content found with given slicer attribute.");
            
            // ReSharper disable once PossibleNullReferenceException
            var variantsOfContent = content.Variants;

            if (variantsOfContent.Any())
            {
                Should(() =>  variantsOfContent.All(c => c.HasSameTypeVarianterAttribute(varianterAttribute)),
                    "Given attribute type should belong to any variant of product as varianter");
            }
            
            Should(() => variantsOfContent.All(v => v.VarianterAttribute != varianterAttribute) ,
                "Same variant already exists with given attribute");
            
            ApplyChange(new VariantAddedToProduct(Id, barcode, slicerAttribute, varianterAttribute));
        }

        public void AddAttributeToContent(AttributeRef attribute)
        {
            Should(() => _attributes.Any(a => a != attribute),
                "Given attribute had already been added to the product");

            ApplyChange(new AttributeAddedToProduct(Id, attribute));
        }
        
        public void Approve()
        {
            Should(() => _contents.Any(), "Product must have at least one content");
            Should(() => _contents.SelectMany(c => c.Variants).Any(), "Product must have at least one variant");
            Should(() => _contents.SelectMany(c => c.Variants).SelectMany(v => v.Images).Any(), "Product must have at least one image");
        } 
        
        public void AssignImage(ImageRef image, AttributeRef varianterAttr)
        {
            Should(() => _contents.SelectMany(c => c.Variants).Any(v => v.HasSameTypeVarianterAttribute(varianterAttr)),
                $"Product does not have any attribute with given attributeId: {varianterAttr.AttributeId}");
            ApplyChange(new ImageAddedToProduct(Id, varianterAttr, image));
        }
    }

   
}