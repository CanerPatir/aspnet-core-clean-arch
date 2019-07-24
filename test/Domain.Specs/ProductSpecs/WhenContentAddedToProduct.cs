using System;
using System.Linq;
using Domain.ProductContext;
using Domain.ProductContext.Events;
using TestBase;
using Xunit;

namespace Domain.Specs.ProductSpecs
{
    public class WhenContentAddedToProduct : SpecBase<Product>
    {
        [Fact]
        public void Content_Should_Be_Added()
        {
            var productId = Random<Guid>();
            var title = Random<string>();
            var description = Random<string>();
            var attributeId = Random<int>();
            var attributeValueId = Random<int>();

            var @event = new ContentAddedToProduct(
                productId,
                title,
                description, new AttributeRef(attributeId, attributeValueId));

            ScenarioForExisting()
                .Given(
                    () => Product.Create(productId, Random<int>(), Random<int>(), Random<string>()))
                .When(
                    product => product.AddContent(title, description, new AttributeRef(attributeId, attributeValueId))
                )
                .Then(@event)
                .AlsoAssert(
                    product => product.Contents
                        .Any(x => x.Title == title)
                );
        }

        [Fact]
        public void Given_Product_Has_DifferentType_Attribute_Then_Business_Exception_Should_Be_Thrown()
        {
            ScenarioForExisting()
                .Given(
                    () =>
                    {
                        var aggregate = Product.Create(Random<Guid>(), Random<int>(), Random<int>(), Random<string>());
                        aggregate.AddContent(Random<string>(), Random<string>(),
                            new AttributeRef(Random<int>(), Random<int>()));
                        return aggregate;
                    })
                .When(
                    product => product.AddContent(Random<string>(), Random<string>(),
                        new AttributeRef(Random<int>(), Random<int>()))
                )
                .ThenThrows<BusinessException>("Given attribute type should belong to any content of product as slicer");
        }

        [Fact]
        public void Given_Product_Has_Same_Attribute_Then_Business_Exception_Should_Be_Thrown()
        {
            var anAttribute = new AttributeRef(Random<int>(), Random<int>());
            ScenarioForExisting()
                .Given(
                    () =>
                    {
                        var aggregate = Product.Create(Random<Guid>(), Random<int>(), Random<int>(), Random<string>());
                        aggregate.AddContent(Random<string>(), Random<string>(), anAttribute);
                        return aggregate;
                    })
                .When(
                    product => product.AddContent(Random<string>(), Random<string>(), anAttribute)
                )
                .ThenThrows<BusinessException>("Same content already exists with given attribute");
        }
    }
}