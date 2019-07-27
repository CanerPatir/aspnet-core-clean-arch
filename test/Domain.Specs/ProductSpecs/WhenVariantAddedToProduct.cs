using System;
using System.Linq;
using Domain.ProductContext;
using Domain.ProductContext.Events;
using TestBase;
using Xunit;

namespace Domain.Specs.ProductSpecs
{
    public class WhenVariantAddedToProduct : SpecBase<Product>
    {
        [Fact]
        public void Variant_Should_Be_Added()
        {
            var slicerAttributeId = Random<int>();
            var slicerAttributeValueId = Random<int>();
            var productId = Random<Guid>();
            var barcode = Random<String>();
            var varianterAttributeId = Random<int>();
            var varianterAttributeValueId = Random<int>();
            
            var @event = new VariantAddedToProduct(productId, barcode, 
                new AttributeRef(slicerAttributeId, slicerAttributeValueId),
                new AttributeRef(varianterAttributeId,  varianterAttributeValueId) );

            ScenarioForExisting()
                .Given(
                    () =>
                    {
                        var aggregate = Product.Create(productId, Random<int>(), Random<int>(), Random<string>());
                        aggregate.AddContent(Random<string>(), Random<string>(), new AttributeRef(slicerAttributeId, slicerAttributeValueId));
                        return aggregate;
                    })
                .When
                (
                    product => product.AddVariant(barcode,
                        new AttributeRef(slicerAttributeId, slicerAttributeValueId), 
                        new AttributeRef(varianterAttributeId, varianterAttributeValueId))
                )
                .Then(@event)
                .AlsoAssert(
                    product => product.Contents.SelectMany(c => c.Variants)
                        .Any(x => x.Barcode == barcode)
                );
        }
        
        [Fact]
        public void Given_Product_Has_No_Content_With_Given_Slicer_Should_Throw_Business_Exception()
        {
            var slicerAttributeId = Random<int>();
            var slicerAttributeValueId = Random<int>();
             var barcode = Random<String>();
            var varianterAttributeId = Random<int>();
            var varianterAttributeValueId = Random<int>();
             
            ScenarioForExisting()
                .Given(
                    () => Product.Create(Random<Guid>(), Random<int>(), Random<int>(), Random<string>()))
                .When
                (
                    product => product.AddVariant(barcode,
                        new AttributeRef(slicerAttributeId, slicerAttributeValueId), 
                        new AttributeRef(varianterAttributeId, varianterAttributeValueId))
                )
                .ThenThrows<BusinessException>("No content found with given slicer attribute.");
        }
    }
}