using System;
using Domain.ProductContext;
using Domain.ProductContext.Events;
using TestBase;
using Xunit;

namespace Domain.Specs.ProductSpecs
{
    public class WhenProductApproved : SpecBase<Product>
    {
        [Fact]
        public void Given_Product_Has_No_Content_Business_Exception_Should_Be_Thrown()
        {
            var productId = Random<Guid>();

            ScenarioForExisting()
                .Given(
                    () =>
                    {
                        var aggregate = Product.Create(productId, Random<int>(), Random<int>(), Random<string>());
                        return aggregate;
                    })
                .When(
                    product => product.Approve())
                .ThenThrows<BusinessException>("Product must have at least one content");
        }

        [Fact]
        public void Given_Product_Has_No_Variant_Business_Exception_Should_Be_Thrown()
        {
            var productId = Random<Guid>();

            ScenarioForExisting()
                .Given(
                    () =>
                    {
                        var aggregate = Product.Create(productId, Random<int>(), Random<int>(), Random<string>());
                        aggregate.AddContent(Random<string>(), Random<string>(),
                            new AttributeRef(Random<int>(), Random<int>()));
                        return aggregate;
                    })
                .When(
                    product => product.Approve())
                .ThenThrows<BusinessException>("Product must have at least one variant");
        }

        [Fact]
        public void Given_Product_Has_No_Image_Business_Exception_Should_Be_Thrown()
        {
            var productId = Random<Guid>();

            ScenarioForExisting()
                .Given(
                    () =>
                    {
                        var aggregate = Product.Create(productId, Random<int>(), Random<int>(), Random<string>());
                        var slicerAttribute = new AttributeRef(Random<int>(), Random<int>());
                        aggregate.AddContent(Random<string>(), Random<string>(), slicerAttribute);
                        aggregate.AddVariant(Random<string>(), slicerAttribute,
                            new AttributeRef(Random<int>(), Random<int>()));
                        return aggregate;
                    })
                .When(
                    product => product.Approve())
                .ThenThrows<BusinessException>("Product must have at least one image");
        }

        [Fact]
        public void Product_Should_Be_Approved()
        {
            var productId = Random<Guid>();
            var @event = new ProductApproved(productId);

            ScenarioForExisting()
                .Given(
                    () =>
                    {
                        var aggregate = Product.Create(productId, Random<int>(), Random<int>(), Random<string>());
                        var slicerAttribute = new AttributeRef(Random<int>(), Random<int>());
                        aggregate.AddContent(Random<string>(), Random<string>(), slicerAttribute);
                        var varianterAttribute = new AttributeRef(Random<int>(), Random<int>());
                        aggregate.AddVariant(Random<string>(), slicerAttribute, varianterAttribute);
                        aggregate.AssignImage(Random<ImageRef>(), varianterAttribute);
                        return aggregate;
                    })
                .When(
                    product => product.Approve())
                .Then(@event)
                .AlsoAssert(product => product.IsApproved);
        }
    }
}